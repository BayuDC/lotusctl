namespace lotusctl.Library {
    public class Service {
        public string DisplayName;
        public string CodeName;

        public bool IsActive;

        public Service(string codeName) {
            this.DisplayName = this.CodeName = codeName;
        }
        public Service(string codeName, string displayName) {
            if (string.IsNullOrEmpty(displayName)) {
                displayName = codeName;
            }
            this.DisplayName = displayName;
            this.CodeName = codeName;
        }
    }
}