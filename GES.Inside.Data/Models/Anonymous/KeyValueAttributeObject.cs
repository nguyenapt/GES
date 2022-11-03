namespace GES.Inside.Data.Models.Anonymous
{
    public class KeyValueAttributeObject<TKey, TValue, TAttribute>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public TAttribute Attribute { get; set; }

        public KeyValueAttributeObject()
        {
        }

        public KeyValueAttributeObject(TKey key, TValue value, TAttribute attribute)
        {
            Key = key;
            Value = value;
            Attribute = attribute;
        }
    }
}
