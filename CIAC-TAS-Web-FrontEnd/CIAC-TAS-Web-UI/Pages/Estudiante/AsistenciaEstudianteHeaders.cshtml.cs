using CIAC_TAS_Service.Sdk;
using CIAC_TAS_Web_UI.Helper;
using CIAC_TAS_Web_UI.ModelViews.Estudiante;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Refit;

namespace CIAC_TAS_Web_UI.Pages.Estudiante
{
    public class AsistenciaEstudianteHeadersModel : PageModel
    {
        [BindProperty]
        public List<AsistenciaEstudianteHeaderModelView> AsistenciaEstudianteHeaderModelView { get; set; } = new List<AsistenciaEstudianteHeaderModelView>();

        [TempData]
        public string Message { get; set; }

        private readonly IConfiguration _configuration;

        public AsistenciaEstudianteHeadersModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var asistenciaEstudianteHeaderServiceApi = RestService.For<IAsistenciaEstudianteHeaderServiceApi>(_configuration.GetValue<string>("ServiceUrl"), new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(HttpContext.Session.GetString(Session.SessionToken))
            });

            var asistenciaEstudianteHeadersResponse = await asistenciaEstudianteHeaderServiceApi.GetAllAsync();

            if (!asistenciaEstudianteHeadersResponse.IsSuccessStatusCode)
            {
                Message = "Ocurrio un error inesperado";

                return Page();
            }

            var asistenciaEstudianteHeaders = asistenciaEstudianteHeadersResponse.Content.Data;
            AsistenciaEstudianteHeaderModelView = asistenciaEstudianteHeaders.Select(x => new AsistenciaEstudianteHeaderModelView
            {
                Id = x.Id,
                ProgramaNombre = x.ProgramaResponse.Nombre,
                GrupoNombre = x.GrupoResponse.Nombre,
                MateriaNombre = x.MateriaResponse.Nombre,
                ModuloNombre = x.ModuloResponse.Nombre,
                InstructorNombre = x.InstructorResponse.Nombres,
            }).ToList();

            return Page();
        }
    }
}
