using AnimalIdentifier.Application.Animals.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AnimalIdentifier.WebUI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    [Authorize]
    public async Task<IActionResult> Index()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = Url.Action("Index", "Home")
            }, OpenIdConnectDefaults.AuthenticationScheme);
        }
        using var httpClient = await GetHttpClientAsync();
        var response = await httpClient.GetAsync("https://localhost:7287/api/animals/GetAll");

        if (!response.IsSuccessStatusCode)
        {
            return new SignOutResult(
            new[] {
                OpenIdConnectDefaults.AuthenticationScheme,
                CookieAuthenticationDefaults.AuthenticationScheme
            });
        }

        var json = await response.Content.ReadAsStringAsync();
        var animals = JsonConvert.DeserializeObject<List<AnimalDto>>(json); // فرض بر اینکه AnimalDto ساخته شده

        return View(animals);
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> GetById([FromBody] DisplayAnimalDto dto)
    {
        var httpClient = await GetHttpClientAsync();
        var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("https://localhost:7287/api/animals/GetById", content);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var animalName = JsonConvert.DeserializeObject<AnimalDto>(json).Name;

            return Json(new { isSuccess = true, name = animalName, message = "عملیات با موفقیت انجام شد." });
        }

        return Json(new { isSuccess = false, message = "خطا د ر انجام عملیات." });
    }
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Edit([FromBody] AnimalDto dto)
    {
        var httpClient = await GetHttpClientAsync();
        var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("https://localhost:7287/api/animals/Update", content);

        if (response.IsSuccessStatusCode)
        {
            return Json(new { isSuccess = true, message = "عملیات ویرایش با موفقیت انجام شد." });
        }

        return Json(new { isSuccess = false, message = "خطا در ویرایش کردن آیتم." });
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create([FromBody] CreateAnimalDto dto)
    {
        var httpClient = await GetHttpClientAsync();
        var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("https://localhost:7287/api/animals/Create", content);

        if (response.IsSuccessStatusCode)
        {
            return Json(new { isSuccess = true, message = "عملیات ثبت با موفقیت انجام شد." });
        }
        return Json(new { isSuccess = false, message = "خطا در ثبت کردن آیتم." });
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete([FromBody] DeleteAnimalDto dto)
    {
        var httpClient = await GetHttpClientAsync();
        var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("https://localhost:7287/api/animals/Delete", content);

        if (response.IsSuccessStatusCode)
        {
            return Json(new { isSuccess = true, message = "عملیات حذف با موفقیت انجام شد." });
        }

        return Json(new { isSuccess = false, message = "خطا در حذف کردن آیتم." });
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        return new SignOutResult(
            new[] {
                OpenIdConnectDefaults.AuthenticationScheme,
                CookieAuthenticationDefaults.AuthenticationScheme
            });
    }

    [NonAction]
    private async Task<HttpClient> GetHttpClientAsync()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

        var httpClient = new HttpClient(handler);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        //string refreshToken = await HttpContext.GetTokenAsync("refresh_token");
        return httpClient;
    }
}
