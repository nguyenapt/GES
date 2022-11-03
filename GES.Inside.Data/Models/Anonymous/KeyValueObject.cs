namespace GES.Inside.Data.Models.Anonymous
{
    public class KeyValueObject<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public KeyValueObject()
        {
        }

        public KeyValueObject(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}
