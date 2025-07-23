
namespace GameZone.Services
{
    public class GameService : IGameService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagesPath;

        public GameService(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.ImagesPath}";
        }
        public IEnumerable<Game> GetAll()
        {
            return _context.Games
                .Include(g => g.Category)
                .Include(g => g.Devices)
                .ThenInclude(d => d.Device)
                .AsNoTracking()
                .ToList();

        }

        public Game? GetById(int id)
        {
            return _context.Games
                .Include(g => g.Category)
                .Include(g => g.Devices)
                .ThenInclude(d => d.Device)
                .AsNoTracking()
                .SingleOrDefault(g => g.Id == id);
        }
        public async Task Create(CreateGameFormViewModel model)
        {
            string coverName = await SaveCover(model.Cover);

            Game game = new()
            {
                Name = model.Name,
                Descriptipn = model.Description,
                CategoryId = model.CategoryId,
                Cover = coverName,
                Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList()
            };

            _context.Add(game);
            _context.SaveChanges();
        }

        public async Task<Game?> Update(EditGameFormViewModel model)
        {
            var game = _context.Games
                .Include(g => g.Devices)
                .SingleOrDefault(g => g.Id == model.Id);

            if (game == null)
                return null;

            var hasNewCover = model.Cover is not null;

            var oldCover = game.Cover;

            game.Name = model.Name;
            game.Descriptipn = model.Description;
            game.CategoryId = model.CategoryId;
            game.Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList();

            if (hasNewCover)
            {
                game.Cover = await SaveCover(model.Cover!);
            }

            var effectedRows = _context.SaveChanges();

            if (effectedRows > 0)
            {
                if (hasNewCover)
                {
                    var cover = Path.Combine(_imagesPath, oldCover);
                    File.Delete(cover);
                }

                return game;
            }
            else
            {
                var cover = Path.Combine(_imagesPath, game.Cover);
                File.Delete(cover);

                return null;
            }
        }

        public bool Delete(int id)
        {
            var isDeleted = false;

            var game = _context.Games.Find(id);

            if (game is null)
                return isDeleted;

            _context.Games.Remove(game);

            var effectedRows = _context.SaveChanges();

            if(effectedRows > 0)
            {
                isDeleted = true;

                var cover = Path.Combine(_imagesPath, game.Cover);
                File.Delete(cover);
            }


            return isDeleted;
        }

        private async Task<string> SaveCover(IFormFile cover)
        {
            string coverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";

            var path = Path.Combine(_imagesPath, coverName);

            using var stream = File.Create(path);
            await cover.CopyToAsync(stream);

            return coverName;
        }
    }
}
