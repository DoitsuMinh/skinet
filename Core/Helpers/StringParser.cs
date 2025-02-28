namespace Core.Helpers
{
    public static class StringParser
    {
        public static List<int> ParseListIds(string ids)
        {
            if (string.IsNullOrEmpty(ids)) return new List<int>();
            try
            {
                return ids.Split(',', StringSplitOptions.RemoveEmptyEntries)
                         .Select(int.Parse)
                         .ToList();
            }
            catch (FormatException ex)
            {
                throw new ArgumentException("Invalid ID format in comma-separated string", nameof(ids), ex);
            }
        }
    }
}
