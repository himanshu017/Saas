namespace AdminPanel.Client.Components.Global
{
    public class ValidationArgs
    {
        public string CurrentChip { get; set; }
        public List<string> Chips { get; set; }
        public List<string> ValidationErrors { get; set; }

        public ValidationArgs(List<string> chips, string currentChip, List<string> validationErrors)
        {
            Chips = chips;
            ValidationErrors = validationErrors;
            CurrentChip = currentChip;
        }
    }
}
