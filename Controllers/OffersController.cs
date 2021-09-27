﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FiberConnectionClient.Models;
using Microsoft.AspNetCore.Mvc;
using JWT;
using Newtonsoft.Json;
using System.Text;

namespace FiberConnectionClient.Controllers
{
    public class OffersController : Controller
    {
        [HttpGet]
        public IActionResult UpdateOffer()
        {
            string AdminToken = HttpContext.Request.Cookies["AdminToken"];
            if (string.IsNullOrEmpty(AdminToken))
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOffer(Offer o)
        {
            string AdminToken = HttpContext.Request.Cookies["AdminToken"];
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AdminToken);
                StringContent addo = new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
                var res = await client.PostAsync("https://localhost:44341/api/Offer", addo);
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("AdminControlOfferDetails");
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AdminControlOfferDetails()
        {
            string AdminToken = HttpContext.Request.Cookies["AdminToken"];
            if (string.IsNullOrEmpty(AdminToken))
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            List<Offer> os = new List<Offer>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AdminToken);
                var res = await client.GetAsync("https://localhost:44341/api/Offer");
                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    os = JsonConvert.DeserializeObject<List<Offer>>(result);

                }
                return View(os);
            }

        }
        [HttpGet]
        public async Task<IActionResult> OfferDetails(int id)
        {
            Offer o = new Offer();
            using (var client = new HttpClient())
            {
                var res = await client.GetAsync("https://localhost:44341/api/Offer/"+id);
                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    o = JsonConvert.DeserializeObject<Offer>(result);

                }
                return View(o);
            }

        }
        [HttpGet]
        public async Task<IActionResult> AdminControlEditOffer(int id)
        {
            string AdminToken = HttpContext.Request.Cookies["AdminToken"];
            if (string.IsNullOrEmpty(AdminToken))
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            Offer o = new Offer();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AdminToken);
                var res = await client.GetAsync("https://localhost:44341/api/Offer/ " + id);
                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    o = JsonConvert.DeserializeObject<Offer>(result);

                }
                return View(o);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AdminControlEditOffer(int id,Offer o)
        {
            string AdminToken = HttpContext.Request.Cookies["AdminToken"];
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AdminToken);
                StringContent edito = new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
                var res = await client.PutAsync("https://localhost:44341/api/Offer?id=" + id, edito);
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("AdminControlOfferDetails");
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AdminControlDeleteOffer(int id)
        {
            string AdminToken = HttpContext.Request.Cookies["AdminToken"];
            if (string.IsNullOrEmpty(AdminToken))
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            Offer o = new Offer();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AdminToken);
                var res = await client.GetAsync("https://localhost:44341/api/Offer" + id);
                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    o = JsonConvert.DeserializeObject<Offer>(result);

                }
                return View(o);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AdminControlDeletePlan(int id, Offer o)
        {
            string AdminToken = HttpContext.Request.Cookies["AdminToken"];
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AdminToken);
                var res = await client.DeleteAsync("https://localhost:44341/api/Offer?id=" + id);
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("AdminControlOfferDetails");
                }
            }
            return RedirectToAction("AdminControlOfferDetails");
        }

    }
}
