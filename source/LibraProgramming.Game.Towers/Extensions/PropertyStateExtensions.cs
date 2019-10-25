using LibraProgramming.Ecs;
using System;
using System.Collections.Generic;

namespace LibraProgramming.Game.Towers.Extensions
{
    public static class PropertyStateExtensions
    {
        /*public static TValue GetValue<TValue>(this PropertyState[] propertyStates, string propertyName)
        {
            var propertyState = Array.Find(
                propertyStates,
                state => String.Equals(state.Name, propertyName, StringComparison.InvariantCulture)
            );

            if (null == propertyState)
            {
                throw new KeyNotFoundException();
            }

            return (TValue)Convert.ChangeType(propertyState.Value, typeof(TValue));
        }*/

        public static TValue GetValue<TValue>(
            this PropertyState[] propertyStates, 
            string propertyName, 
            Converter<string, TValue> converter = null)
        {
            var propertyState = Array.Find(
                propertyStates,
                state => String.Equals(state.Name, propertyName, StringComparison.InvariantCulture)
            );

            if (null == propertyState)
            {
                throw new KeyNotFoundException();
            }

            if (null == converter)
            {
                converter = value => (TValue) Convert.ChangeType(value, typeof(TValue));
            }

            //return (TValue)Convert.ChangeType(propertyState.Value, typeof(TValue));

            return converter.Invoke(propertyState.Value);
        }
    }
}