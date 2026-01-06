namespace Leoni.Utils
{
    public class ListWrapper<T> where T : class
    {

        public string? Message { get; set; }
        public IReadOnlyList<T> Items { get; set; } = new List<T>();

        public int count => Items.Count;
        public List<string>? Errors { get; set; }
    }
}
