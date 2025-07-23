namespace GameZone.Services
{
    public interface IGameService
    {
        IEnumerable<Game> GetAll();

        Game? GetById(int id);
        Task Create(CreateGameFormViewModel mdeol);

        Task<Game?> Update(EditGameFormViewModel mdeol);
        bool Delete(int id);
    }
}
