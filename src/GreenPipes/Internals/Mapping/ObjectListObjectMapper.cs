﻿namespace GreenPipes.Internals.Mapping
{
    using System.Collections.Generic;
    using Reflection;


    public class ObjectListObjectMapper<T, TElement> :
        IObjectMapper<T>
    {
        readonly IObjectConverter _converter;
        readonly ReadWriteProperty<T> _property;

        public ObjectListObjectMapper(ReadWriteProperty<T> property,
            IObjectConverter converter)
        {
            _property = property;
            _converter = converter;
        }

        public void ApplyTo(T obj, IObjectValueProvider valueProvider)
        {
            IArrayValueProvider values;
            if (!valueProvider.TryGetValue(_property.Property.Name, out values))
                return;

            var elements = new List<TElement>();

            for (var i = 0;; i++)
            {
                IObjectValueProvider elementValueProvider;
                if (!values.TryGetValue(i, out elementValueProvider))
                    break;

                var element = (TElement)_converter.GetObject(elementValueProvider);
                elements.Add(element);
            }

            _property.Set(obj, elements);
        }
    }
}
