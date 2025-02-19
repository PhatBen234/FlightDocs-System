namespace FlightDocs_System.ViewModels.AllDocuments
{
    public class Document_GroupViewModel
    {
        public int ID_Document_Group { get; set; }
        public int ID_GroupPermission { get; set; }
        public string? GroupPermissionName { get; set; }
        /*public bool IsReadModify { get; set; }
        public bool IsRead { get; set; }
        public bool NoPermission { get; set; }*/
        // Thêm các trường khác của Document_Group bạn muốn hiển thị
    }
}
