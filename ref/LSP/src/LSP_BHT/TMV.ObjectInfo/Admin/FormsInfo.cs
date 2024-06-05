namespace TMV.ObjectInfo
{
    public class FormsInfo
    {
        public int ID { get; set; }
        public string FORM_NAME { get; set; }
        public string FORM_TEXT { get; set; }
      
        public FormsInfo()
        {
        }

        public FormsInfo(int ID, string FORM_NAME, string FORM_TEXT)
        {
            this.ID = ID;
            this.FORM_NAME = FORM_NAME;
            this.FORM_TEXT = FORM_TEXT;
           
        }
    }
}
