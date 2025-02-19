﻿namespace SampleApp.Shared.Abstractions.Records
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    // source: https://github.com/jhewlett/ValueObject
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        private List<FieldInfo> _fields;
        private List<PropertyInfo> _properties;

        public bool Equals(ValueObject obj)
        {
            return Equals(obj as object);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return GetProperties().All(p => PropertiesAreEqual(obj, p))
                && GetFields().All(f => FieldsAreEqual(obj, f));
        }

        public override int GetHashCode()
        {
            unchecked //allow overflow
            {
                int hash = 17;
                foreach (var prop in GetProperties())
                {
                    var value = prop.GetValue(this, null);
                    hash = HashValue(hash, value);
                }

                foreach (var field in GetFields())
                {
                    var value = field.GetValue(this);
                    hash = HashValue(hash, value);
                }

                return hash;
            }
        }

        public static bool operator ==(ValueObject obj1, ValueObject obj2)
        {
            if (Equals(obj1, null))
            {
                if (Equals(obj2, null))
                {
                    return true;
                }

                return false;
            }

            return obj1.Equals(obj2);
        }

        public static bool operator !=(ValueObject obj1, ValueObject obj2)
        {
            return !(obj1 == obj2);
        }

        private bool FieldsAreEqual(object obj, FieldInfo f)
        {
            return Equals(f.GetValue(this), f.GetValue(obj));
        }

        private IEnumerable<FieldInfo> GetFields()
        {
            if (_fields == null)
            {
                _fields = GetType()
                    .GetFields(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => p.GetCustomAttribute(typeof(IgnoreMemberValueAttribute)) == null)
                    .ToList();
            }

            return _fields;
        }

        private IEnumerable<PropertyInfo> GetProperties()
        {
            if (_properties == null)
            {
                _properties = GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => p.GetCustomAttribute(typeof(IgnoreMemberValueAttribute)) == null)
                    .ToList();

                // Not available in Core
                // !Attribute.IsDefined(p, typeof(IgnoreMemberAttribute))).ToList();
            }

            return _properties;
        }

        private int HashValue(int seed, object value)
        {
            var currentHash = value != null
                ? value.GetHashCode()
                : 0;

            return (seed * 23) + currentHash;
        }

        private bool PropertiesAreEqual(object obj, PropertyInfo p)
        {
            return Equals(p.GetValue(this, null), p.GetValue(obj, null));
        }
    }
}
