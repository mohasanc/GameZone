using System.Threading.Tasks;

namespace GameZone.Controllers
{
    public class GamesController : Controller
    {
        private readonly ICategoriesService _categoriesService;
        private readonly IDevicesService _devicesService;
        private readonly IGameService _gameService;

        public GamesController(ICategoriesService categoriesService,
            IDevicesService devicesService,
            IGameService gameService,
            AppDbContext context)
        {
            _categoriesService = categoriesService;
            _devicesService = devicesService;
            _gameService = gameService;
        }
        public IActionResult Index()
        {
            var games = _gameService.GetAll();

            return View(games);
        }

        public IActionResult Details(int id)
        {
            var game = _gameService.GetById(id);

            if (game is null)
            {
                return NotFound();
            }
            return View(game);
        }

        [HttpGet]
        public IActionResult Create()
        {
            CreateGameFormViewModel viewModel = new()
            {
                Categories = _categoriesService.GetSelectListItems(),

                Devices = _devicesService.GetSelectListItems()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoriesService.GetSelectListItems();

                model.Devices = _devicesService.GetSelectListItems();

                return View(model);
            }

            // Save Game in database
            // Save cover in sever
            await _gameService.Create(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var game = _gameService.GetById(id);

            if (game is null)
            {
                return NotFound();
            }

            EditGameFormViewModel viewModel = new()
            {
                Id = id,
                Name = game.Name,
                Description = game.Descriptipn,
                CategoryId = game.CategoryId,
                SelectedDevices = game.Devices.Select(d => d.DeviceId).ToList(),
                Categories = _categoriesService.GetSelectListItems(),
                Devices = _devicesService.GetSelectListItems(),
                CurrentCover = game.Cover
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoriesService.GetSelectListItems();
                model.Devices = _devicesService.GetSelectListItems();
                return View(model);
            }

            var game = await _gameService.Update(model);

            if (game is null)
                return BadRequest();


            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var isDeleted = _gameService.Delete(id);

            return isDeleted ? Ok() : BadRequest();
        }
    }
}
