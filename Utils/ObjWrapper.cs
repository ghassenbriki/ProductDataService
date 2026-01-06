namespace Leoni.Utils
{
    public class ObjWrapper<T> where T : class
    {
        public string? Message { get; set; }
        public T? Item { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
    }
}
