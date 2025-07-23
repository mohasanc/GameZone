namespace GameZone.Services
{
    public interface ICategoriesService
    {
        IEnumerable<SelectListItem> GetSelectListItems();
    }
}
