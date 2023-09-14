namespace Models
{
    public class Document
    {
        public string Filename { get; set; }
        public DocumentBody Body { get; set; }
    }
}

public class DocumentBody
{
    public string name { get; set; }
    public int age { get; set; }
    public string city { get; set; }
    public string state { get; set; }
    public string zip { get; set; }
    public string country { get; set; }
}