using API.Models;
using Client.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages
{
    public partial class AddCoffeeShop
    {
        [BindProperty]
        private CoffeeShopModel Shop { get; set; } = new();
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private IConfiguration Config { get; set; }
        [Inject] private ITokenService TokenService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var tokenResponse = await TokenService.GetToken("CoffeeAPI.write");
            HttpClient.SetBearerToken(tokenResponse.AccessToken);
        }
        
        public async Task<IActionResult> OnPost(string redirectUri = null)
        {
            if (string.IsNullOrWhiteSpace(redirectUri))
            {
                redirectUri = "~/";
            }

            if (Shop.Name == null)
            {
                return new RedirectToPageResult("addcoffeeshop");
            }

            var json = JsonConvert.SerializeObject(Shop);
            var data = new StringContent(json, encoding: Encoding.UTF8, "application/json");
            
            var result = await HttpClient.PostAsync(Config["apiUrl"] + "/api/CoffeeShop/addcoffeeshop", data);

            if (!result.IsSuccessStatusCode)
            {
                Shop = null; 
                return new RedirectToPageResult("~/addcoffeeshop");
            }
            else
            {
                return new RedirectToPageResult(redirectUri);
            }
        }
    }
}
