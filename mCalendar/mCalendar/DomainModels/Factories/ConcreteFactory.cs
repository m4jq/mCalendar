using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using mCalendar.DomainModels.Interfaces;
using mCalendar.Exceptions;

namespace mCalendar.DomainModels.Factories
{
    public abstract class ConcreteFactory<T>
    {
        Dictionary<string, Type> _reminderTypes;

        protected ConcreteFactory()
        {
            LoadTypes();
        }

        public T CreateInstance(string name) 
        {
            Type t = GetTypeToCreate(name.ToLower());
 
            if (t == null)
                throw new InvalidTypeException();

            return (T)Activator.CreateInstance(t);
        }
 
        private Type GetTypeToCreate(string reminderType)
        {
            foreach (var reminder in _reminderTypes)
            {
                if (reminder.Key.Contains(reminderType))
                {
                    return _reminderTypes[reminder.Key];
                }
            }
 
            return null;
        }
 
        private void LoadTypes()
        {
            _reminderTypes = new Dictionary<string, Type>();
 
            Type[] typesInThisAssembly = Assembly.GetExecutingAssembly().GetTypes();
 
            foreach (Type type in typesInThisAssembly)
            {
                if (type.GetInterface(typeof(T).ToString()) != null)
                {
                    _reminderTypes.Add(type.Name.ToLower(), type);
                }
            }
        }
    }
}