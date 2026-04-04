class ClassA{
    public IList<string> Strings{get;} = new List<string>();
    public Type1 Prop1{get;set;}
    public Type2 field1;
}

class Type1 {
    public int value1{get;set;}
}

class Type2{
    public string string1{get;set;}
    public ExternalType Prop2 {get;set;}
}
