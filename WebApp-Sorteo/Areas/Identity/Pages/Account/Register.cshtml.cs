// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using WebApp_Sorteo.Helpers;
using WebApp_Sorteo.Models;
using WebApp_Sorteo.Models.Helpers;

namespace WebApp_Sorteo.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;


        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            RoleManager<IdentityRole> role,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = role;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public class InputModel
        {
            [Required(ErrorMessage = "El campo {0} es requerido, completar")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "El campo {0} es requerido, completar")]
            [StringLength(100, ErrorMessage = "El campo {0} debe tener al menos {2} y un máximo de {1} caracteres.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; }

            [Required(ErrorMessage = "El campo {0} es requerido, completar")]

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar la contraseña")]
            [Compare("Password", ErrorMessage = "Las contraseñas no coinciden, intenta de nuevo.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Número de teléfono")]
            [StringLength(50, ErrorMessage = "El campo {0} debe tener al menos {2} y un máximo de {1} caracteres.", MinimumLength = 10)]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "El campo {0} es requerido")]
            [Display(Name = "Nombres del Usuario")]
            public string Nombres { get; set; }

            [Required(ErrorMessage = "El campo {0} es requerido, por favor revisar la información")]
            public string Apellidos { get; set; }
            [Required(ErrorMessage = "El campo {0} es requerido, completar")]

            public string Direccion { get; set; }
            [Required(ErrorMessage = "El campo {0} es requerido, completar")]

            public string Ciudad { get; set; }
            [Required(ErrorMessage = "El campo {0} es requerido, completar")]

            public string Pais { get; set; }

            [Required(ErrorMessage = "El campo {0} es requerido, completar")]
            public string Role { get; set; }

            public List<SelectListItem> ListaRol { get; set; }
            [Required(ErrorMessage = "El campo {0} es requerido, completar")]
            public string Documento { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            if (User.IsInRole(Roles.Role_Admin))
            {
                Input = new InputModel
                {
                    ListaRol = _roleManager.Roles.Select(x => x.Name).Select(n =>
                    new SelectListItem
                    {
                        Text = n,
                        Value = n
                    }).ToList(),
                };
            }
            else
            {
                Input = new InputModel()
                {
                    ListaRol = _roleManager.Roles.Where(x => x.Name != Roles.Role_Admin).Select(n => n.Name).Select(l => new SelectListItem
                    {
                        Text = l,
                        Value = l
                    }).ToList()
                };
            }

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new Usuario
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    PhoneNumber = Input.PhoneNumber,
                    Nombres = Input.Nombres,
                    Apellidos = Input.Apellidos,
                    Direccion = Input.Direccion,
                    Ciudad = Input.Ciudad,
                    Pais = Input.Pais,
                    Role = Input.Role,
                    Documento = Input.Documento
                };

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    TempData[DS.Exitosa] = "Se ha creado un usuario nuevo";

                    if (!await _roleManager.RoleExistsAsync(Roles.Role_Admin))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(Roles.Role_Admin));
                    }
                    if (!await _roleManager.RoleExistsAsync(Roles.Role_Cliente))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(Roles.Role_Cliente));
                    }
                    if (user.Role is null)
                    {
                        await _userManager.AddToRoleAsync(user, Roles.Role_Cliente);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, user.Role);
                    }
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId, code, returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirme su correo electrónico",
                        $"Por favor, confirme su cuenta <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>haciendo clic aquí</a>.");


                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        TempData[DS.Exitosa] = "Se ha creado el usuario";
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        if (user.Role == null)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            TempData[DS.Exitosa] = "Usuario creado";
                            return RedirectToAction("Index", "Usuarios");
                        }
                    }
                }
                Input = new InputModel()
                {
                    ListaRol = _roleManager.Roles.Where(i => i.Name != Roles.Role_Admin).Select(s => s.Name).Select(l => new SelectListItem
                    {
                        Text = l,
                        Value = l
                    }).ToList(),
                };
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            TempData[DS.Error] = "No se pudo crear el usuario";
            return LocalRedirect(returnUrl);
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
