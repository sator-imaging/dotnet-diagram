namespace Foo
{
    class ClassA
    {
        public IList<string> Strings { get; } = new List<string>();
        public Bar.Type1 Prop1 { get; set; }
        public Bar.Type2 field1;
    }
}

namespace Bar
{
    class Type1
    {
        public int Value1 { get; set; }
    }

    class Type2
    {
        public string String1 { get; set; }
        public ExternalType Prop2 { get; set; }
    }
}
