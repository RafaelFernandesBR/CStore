using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using LojaAc.Data.IModels;

namespace LojaAc.Controllers
{
    public class GeralController : Controller
    {
        private readonly ILogger<GeralController> _logger;
        private readonly IDataModel _dataModel;
        private readonly IAutenticaUserModel _autenticaUser;
        private readonly ILogsData _logs;

        public GeralController(ILogger<GeralController> logger,
            IDataModel dataModel,
            IAutenticaUserModel autenticaUser,
            ILogsData logs)
        {
            _logger = logger;
            _dataModel = dataModel;
            _autenticaUser = autenticaUser;
            _logs = logs;
        }

        [Route("allapp")]
        [HttpGet("[action]")]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> allApps()
        {
            var final = _dataModel.GetAll();

            return View(final);
        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<IActionResult> enviaDados(string nome, string descricao, string url)
        {
            //inserte data
            _dataModel.Insert(nome, descricao, url);
            _dataModel.InsertLog(User.Identity.Name, $"Aplicativo {nome} cadastrado.");

            ViewBag.NomeInc = nome;

            return View();
        }

        [Route("app/{id}")]
        [HttpGet("[action]")]
        [ResponseCache(Duration = 30)]
        public async Task<IActionResult> getId(int id)
        {
            //get data
            var final = _dataModel.GetId(id);

            return View(final);
        }

        [Route("app/{id}/clicke")]
        [HttpGet("[action]")]
        public async Task<IActionResult> RedirectToId(int id)
        {
            //get data
            var final = _dataModel.GetId(id);

            if (final.clickes == 0)
            {
                _dataModel.UpdateClickes(1, id);
            }
            else
            {
                int clickes = Convert.ToInt32(final.clickes);
                clickes = clickes + 1;

                _dataModel.UpdateClickes(clickes, id);
            }

            return Redirect(final.url);
        }

        [Authorize]
        [Route("app/delet/{id}")]
        [HttpGet("[action]")]
        public IActionResult deleteId(int id)
        {
            //delete data
            _dataModel.Delete(id);
            _dataModel.InsertLog(User.Identity.Name, $"Aplicativo {id} apagado.");

            ViewBag.Del = "Aplicativo apagado";

            return View();
        }

        [HttpPost("[action]")]
        public IActionResult ValidaUser(string user, string psw)
        {
            //validar o usuario
            string senha = _autenticaUser.Criptografa(psw);

            bool valido = _autenticaUser.AutenticaUser(user, senha);
            if (valido)
            {
                Login(user);
                //obter o horario atual
                string horario = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                _dataModel.InsertLog(user, $"Usuário logado com sucesso: {horario}.");

                return RedirectToAction("Cadastra", "Home");
            }
            else
            {
                return RedirectToAction("DefBoard", "Home");
            }
        }

        [Authorize]
        [HttpPost("[action]")]
        public IActionResult CadastraUser(string nome, string user, string psw)
        {
            //criptografar a senha
            string senha = _autenticaUser.Criptografa(psw);

            //cadastrar usuario 
            _dataModel.InsertLog(User.Identity.Name, $"Usuário {user} criado para a conta de {nome}.");
            _autenticaUser.CadastraUser(nome, user, senha);

            ViewBag.Nome = nome;

            return View();
        }

        [Route("pesq")]
        [HttpPost("[action]")]
        [ResponseCache(Duration = 120)]
        public async Task<IActionResult> Pesquiza(string pesq)
        {
            //pesquizar o app no banco de dados e retornar a lista de apps
            var final = _dataModel.GetAll();
            List<ListaDeFiltros> Resultados = new List<ListaDeFiltros>();

            foreach (var pesquizando in final)
            {
                //verifica se o nome é igual ou pelomenos parecido com pesq com margem de erro
                if (pesquizando.Nome.Contains(pesq))
                {
                    ListaDeFiltros Resultado = new ListaDeFiltros();
                    Resultado.id = pesquizando.id;
                    Resultado.nome = pesquizando.Nome;
                    Resultado.descricao = pesquizando.descricao;
                    Resultados.Add(Resultado);
                }
            }

            ViewBag.Pesquizado = pesq;

            return View(Resultados);
        }

        [Authorize]
        public IActionResult LimpaLogs()
        {
            _logs.LimpaLogs();
            return RedirectToAction("Logs", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private async void Login(string user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user),
                new Claim(ClaimTypes.Role, "Usuario_AdmAc")
            };

            var identidadeDeUsuario = new ClaimsIdentity(claims, "Login");
            ClaimsPrincipal claimPrincipal = new ClaimsPrincipal(identidadeDeUsuario);

            var propriedadesDeAutenticacao = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTime.Now.ToLocalTime().AddHours(2),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, propriedadesDeAutenticacao);
        }

    }
}
