using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimilaritySearch.Application.DTOs;
using SimilaritySearch.Infrastructure.Database;

namespace SimilaritySearch.Tests;

[CollectionDefinition("IntegrationTests")]
public class SharedTestCollection : ICollectionFixture<IntegrationTestWebApplicationFactory> { }

[Collection("IntegrationTests")]
public class AdIntegrationTests
{
    private readonly IntegrationTestWebApplicationFactory _factory;
    private readonly HttpClient _client;
    
    public AdIntegrationTests(IntegrationTestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
    }
    
    [Fact]
    public async Task PostAds_GenerateEmbeddings_DetectReUploads()
    {
        var adsToCreate = new List<CreateAdCommand>
        {
            new() { UserName = "admin", BrandModel = "ŠKODA SUPERB 3", Motor = "2.0TSi 140kW", PhoneNumber = "123 456 789", Email = "jan.novak@seznam.cz", Location = "Praha", Price = 900000, Description = "XXX POZOR NEJEDNÁ SE O DOVOZ ZE ZAHRANIČÍ!xxx- nehrozí riziko stočeného tachometru a právních vad jako u dovozu!\\n\\nProdám i na splátky-ŠKODA SUPERB 3 2.0TSi 140kW Koup.ČR,1.maj.,DPH,Servisní kniha\\n\\nPro podnikatele a firmy odpočet DPH!!!\\n\\nxxx s možností ZÁRUKY až na 24měsíců!!! xxx- ojetý vůz se ZÁRUKOU jako na NOVÝ!!!\\n\\nr.v.2020\\n140kW-190koní\\nnajeto-174.000km\\nmodrá\\nBENZÍN\\nPravidelný servis pouze autorizovaný servis ŠKODA\\n\\naut. převodovka, 7 rychlostních stupňů, 7x airbag, airbag řidiče, ABS, brzdový asistent, stabilizace podvozku (ESP), EDS, protiprokluzový systém kol (ASR), nouzové brzdění (PEBS), asistent rozjezdu do kopce (HSA), aut. zabrzdění v kopci, posilovač řízení, dvouzónová klimatizace, aut. klimatizace, adaptivní tempomat, LED denní svícení, alu kola, plní 'EURO VI', palubní počítač, hlasové ovládání palubního počítače, dotykové ovládání palubního počítače, elektronická ruční brzda, parkovací senzory přední, parkovací senzory zadní, parkovací asistent, bezklíčové startování, senzor světel, senzor stěračů, nastavitelný volant, multifunkční volant, deaktivace airbagu spolujezdce, hands free, Android Auto, Apple CarPlay, bluetooth, el. okna, el. zrcátka, startování tlačítkem, zaslepení zámků, imobilizér, alarm, centrál dálkový, isofix, vyhřívaná sedadla, výškově nastavitelná sedadla, výškově nastavitelné sedadlo řidiče, senzor tlaku v pneumatikách, přední světla LED, zadní světla LED, mlhovky,SB, AUX, autorádio, digitální příjem rádia (DAB), venkovní teploměr, vyhřívaná zrcátka, klimatizovaná přihrádka, dělená zadní sedadla, zadní loketní opěrka, tónovaná skla\\n\\nKoupeno v ČR\\nPo 1.majiteli\\nServisní kniha\\nMožnost odpočtu DPH!\\nPravidelný servis pouze autorizovaný servis ŠKODA\\n\\nPOSKYTUJI 100% GARANCI PůVODU A NAJETÝCH KILOMETRů-Doloženo servisní knihou,předávacím protokolem,CEBII!!\\n\\n!!!Bojíte se, že v případě nečekané poruchy auta budete muset sahat hluboko do kapsy?\\nU mě nemusíte! Můžeme sjednat záruku až na 36měsíců,která Vám kryje náhlé a nepředvídatelné vnitřní mechanické nebo elektrické poruchy pojištěného motorového vozidla!!!\\n\\nMÁM KRYTOU PRODEJNU V OLOMOUCI! Prodávám jen vozidla pocházející z ČR, po prvním majiteli, se servisní knížkou, s jasnou servisní historií a garancí najetých kilometrů! VŽDY VíTE CO KUPUJETE!\\n\\nDo BANKY PRO PENÍZE NEMUSÍTE! -Stačí Vám složit pouze 10% z ceny nebo Váš starý vůz na protiúčet!\\n\\nPro osobní prohlídku, zkušební jízdu a více info volejte prosím-123 456 789!jan.novak@seznam.cz" },
            new() { UserName = "admin", BrandModel = "ŠKODA SUPERB 3", Motor = "2.0TSi 140kW", PhoneNumber = "123 456 789", Email = "jan.novak@seznam.cz", Location = "Praha", Price = 900000, Description = "UPOZORNĚNÍ: Nejedná se o dovoz ze zahraničí – nehrozí tedy riziko stočeného tachometru ani právních vad, které se u dovozů mohou objevit.\\n\\nNabízím k prodeji (možné i na splátky): ŠKODA SUPERB 3 2.0 TSI 140 kW, koupeno v ČR, první majitel, možnost odpočtu DPH, servisní kniha.\\n\\nVhodné pro podnikatele a firmy – možnost odpočtu DPH.\\n\\nMožnost sjednání záruky až na 24 měsíců – ojetý vůz se zárukou jako na nový.\\n\\nRok výroby: 2020\\nVýkon: 140 kW (190 koní)\\nNajeto: 174 000 km\\nBarva: modrá\\nPalivo: benzín\\nServis: pravidelně pouze v autorizovaném servisu ŠKODA\\n\\nVýbava:\\nautomatická převodovka (7 stupňů), 7x airbag, ABS, brzdový asistent, ESP, EDS, ASR, nouzové brzdění (PEBS), asistent rozjezdu do kopce (HSA), posilovač řízení, dvouzónová automatická klimatizace, adaptivní tempomat, LED denní svícení, alu kola, norma EURO VI, palubní počítač (hlasové i dotykové ovládání), elektronická ruční brzda, parkovací senzory (přední i zadní) + parkovací asistent, bezklíčové startování, senzory světel a stěračů, multifunkční volant, handsfree, Android Auto, Apple CarPlay, Bluetooth, el. okna a zrcátka, startování tlačítkem, imobilizér, alarm, centrální zamykání na dálku, ISOFIX, vyhřívaná sedadla, nastavitelná sedadla, kontrola tlaku v pneumatikách, LED světla vpředu i vzadu, mlhovky, AUX, autorádio, DAB rádio, venkovní teploměr, vyhřívaná zrcátka, klimatizovaná přihrádka, dělená zadní sedadla, zadní loketní opěrka, tónovaná skla\\n\\nDalší informace:\\nkoupeno v ČR\\npo 1. majiteli\\nservisní kniha\\nmožnost odpočtu DPH\\npravidelný autorizovaný servis ŠKODA\\n\\nGarantuji 100% původ a najeté kilometry – doloženo servisní knihou, předávacím protokolem a CEBIA prověřením.\\n\\nObáváte se nečekaných oprav? Je možné sjednat záruku až na 36 měsíců, která pokrývá náhlé mechanické i elektrické poruchy.\\n\\nProdejna v Olomouci (krytá). Nabízím pouze vozy z ČR, po prvním majiteli, se servisní historií a garantovanými kilometry – vždy víte, co kupujete.\\n\\nFinancování: není nutné řešit banku – stačí akontace od 10 % ceny nebo možnost protiúčtu za váš stávající vůz.\\n\\nPro více informací, domluvení prohlídky nebo zkušební jízdy volejte: 123 456 789 nebo email: jan.novak@seznam.cz" },
            new() { UserName = "admin", BrandModel = "Nissan Terrano 2", Motor = "2.7TD 92kw", PhoneNumber = "987 654 321", Email = null, Location = "Kladno", Price = 23000, Description = "Prodám Nissana Terrano 2, 2.7TD, 92kw.Rok výroby 2004.K vozidlu 2 ks klíčů, v současné době bez TP a SPZ. Vozidlo plně pojízdné, vše funkční, proto prodávám pouze jako celek.Rozumne dohodě se nebráním." },
            new() { UserName = "admin", BrandModel = "Nissan Terrano 2", Motor = "2.7TD 92kw", PhoneNumber = "987 654 321", Email = null, Location = "Kladno", Price = 23000, Description = "Nabízím k prodeji Nissana Terrano 2, 2.7TD, 92kw.Rok výroby 2004.K vozidlu 2 ks klíčů a sada alu kol s letní pneu (znimní jsou obuté), v současné době bez TP a SPZ. Vozidlo plně pojízdné, vše funguje. Cena 23000 Kč. Volejte 987 654 321." },
            new() { UserName = "admin", BrandModel = "Nissan Terrano 2", Motor = "2.7TD 74kw", PhoneNumber = "987 654 321", Email = null, Location = "Kladno", Price = 21000, Description = "Prodám Nissana Terrano 2, 2.7TD, 74kw.Rok výroby 2004.K vozidlu 2 ks klíčů, v současné době bez TP a SPZ. Vozidlo plně pojízdné, vše funkční, proto prodávám pouze jako celek.Rozumne dohodě se nebráním." },
        };

        var createdAdIds = new List<Guid>();

        foreach (var adCommand in adsToCreate)
        {
            var response = await _client.PostAsJsonAsync("/ads", adCommand);
            response.EnsureSuccessStatusCode();

            var createdAd = await response.Content.ReadFromJsonAsync<AdDto>();
            Assert.NotNull(createdAd);
            createdAdIds.Add(createdAd.Id);

            using var iScope = _factory.Services.CreateScope();
            var iDbContext = iScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var cts = new CancellationTokenSource(TimeSpan.FromMinutes(10));
            var isReady = false;

            while (!cts.IsCancellationRequested && !isReady)
            {
                var dbAd = await iDbContext.Ads.FirstOrDefaultAsync(a => a.Id == createdAd.Id, cancellationToken: cts.Token);
                if (dbAd != null && dbAd.ReadyToBePresented)
                {
                    isReady = true;
                    break;
                }
                
                await Task.Delay(2000, cts.Token);
                iDbContext.ChangeTracker.Clear();
            }
            
            Assert.True(isReady);
        }
        
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        var finalAds = await dbContext.Ads.Where(a => createdAdIds.Contains(a.Id)).ToListAsync();
        finalAds = finalAds.OrderBy(a => createdAdIds.IndexOf(a.Id)).ToList();

        Assert.Equal(5, finalAds.Count);
        
        foreach (var ad in finalAds)
        {
            Assert.NotNull(ad.DescriptionEmbedding);
        }

        Assert.False(finalAds[0].IsReupload);
        Assert.True(finalAds[1].IsReupload);
        Assert.False(finalAds[2].IsReupload);
        Assert.True(finalAds[3].IsReupload);
        Assert.False(finalAds[4].IsReupload);
    }
}