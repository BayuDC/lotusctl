namespace lotusctl.Library {
    public class Service {
        public string DisplayName;
        public string CodeName;

        public Service(string codeName) {
            this.DisplayName = this.CodeName = codeName;
        }
        public Service(string codeName, string displayName) {
            this.DisplayName = displayName;
            this.CodeName = codeName;
        }
    }
}