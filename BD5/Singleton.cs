using System;
using System.Collections.Generic;
using System.Reflection;

namespace BD5
{
    public class Singleton
    {
        // В словаре храним УНИКАЛЬНЫЕ типы их ЕДИНСТВЕННЫЕ экзмпляры, т.е. тех, кто будет синглтоном
        private static readonly Dictionary<Type, object> instances = new Dictionary<Type, object>();
        // lockObject - обеспечивает потокобезопасность
        private static readonly object lockObject = new object();

        protected Singleton() { }

        public static T Instance<T>() where T : class
        {
            // Лочим (запрещаем обращаться к нему из других потоков)
            // объект при попытки его получить
            lock (lockObject)
            {
                // Если обеъет НЕ создан ещё
                if (!instances.ContainsKey(typeof(T)))
                {
                    // Создаём
                    T instance = CreateInstance<T>();
                    // И сохраняем в словарь
                    instances[typeof(T)] = instance;
                }
                // Возвращаем экземпляр объекта по его типу
                return (T)instances[typeof(T)];
            }
        }

        private static T CreateInstance<T>() where T : class
        {
            // Через механизм рефлексии пытаемся найти конструктор у объекта типа T
            ConstructorInfo constructor
                // BindingFlags это параметры поиска, например, BindingFlags.NonPublic указывает на наличие
                // закрытого конструктора. Можно посмотреть аргументы метода GetConstructor дабы лучше понять поиск
                // какого именно конструктора производиться
                = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
            // Если конструктор не нашли, выбрасываем исключение
            if (constructor == null)
            {
                throw new MissingMethodException($"Type {typeof(T).FullName} does not have a private parameterless constructor.");
            }
            // Если нашли, то возвращаем объект вызвав его найденный конструктор
            return (T)constructor.Invoke(null);
        }
    }
}
