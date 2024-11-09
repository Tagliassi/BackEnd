namespace BACK_END_PROJECT.API.users
{
    public enum SortDir
    {
        ASC,
        DESC
    }

    public static class SortDirExtensions
    {
        // Método para obter o valor do enum a partir de uma string
        public static SortDir? GetByName(string name)
        {
            if (string.IsNullOrEmpty(name)) return SortDir.ASC;

            // Tenta converter a string para o enum correspondente
            if (Enum.TryParse(name, true, out SortDir result))
            {
                return result;
            }

            return null; // Caso o nome não corresponda a um valor válido do enum
        }
    }
}
