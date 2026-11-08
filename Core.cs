using HarmonyLib;
using MelonLoader;
using Restory.Gameplay.Statistics;
using Restory.Gameplay.TimeSystems;
using Restory.Gameplay.WorkshopRatings;
using Restory.UI.Presenters.Notepad;
using TMPro;
using UnityEngine;

[assembly: MelonInfo(typeof(RestoryAchievementStats.Core), "RestoryAchievementStats", "1.0.0", "Jerome", null)]
[assembly: MelonGame("Mandragora", "Restory")]

namespace RestoryAchievementStats
{
    [HarmonyPatch(typeof(GUI_NotepadWindow), "SwitchPanels")]
    public class Core : MelonMod
    {
        private static GameStatisticsService _gameStats;
        private static GameCalendar _gameCalendar;
        private static WorkshopRatingsService _ratingsService;

        private static void Postfix(bool isWorkspaceEmpty)
        {
            if (!isWorkspaceEmpty) return;

            if(_gameStats == null)
                _gameStats = GameObject.FindFirstObjectByType<GameStatisticsService>();

            if(_gameCalendar == null)
                _gameCalendar = GameObject.FindFirstObjectByType<GameCalendar>();

            if (_ratingsService == null)
                _ratingsService = GameObject.FindFirstObjectByType<WorkshopRatingsService>();

            if (_gameStats == null || _gameCalendar == null || _ratingsService == null) return;

            var emptySurfaceMessage = GameObject.Find("EpmtySurfaceMessage");
            if (emptySurfaceMessage == null) return;

            var headingObj = emptySurfaceMessage.transform.Find("Empty desk");
            if (headingObj == null) return;

            var descriptionObj = emptySurfaceMessage.transform.Find("Description");
            if (descriptionObj == null) return;

            var partsCleaned = _gameStats.DeviceActions.CleanedElementsCount;
            var partsCleanedPlural = partsCleaned != 1 ? "s" : "";

            var daysAwake = _gameCalendar.CurrentDayNumber - _gameStats.General.LastDayOfSkippingDay;
            var daysAwakePlural = daysAwake != 1 ? "s" : "";

            var boxesPurchased = _gameStats.Store.PurchasedElementsBoxesInFleaMarketCount;
            var boxesPurchasedPlural = boxesPurchased != 1 ? "es" : "";

            var ordersCompleted = _gameStats.Orders.CompletedEmailOrdersCount;
            var ordersCompletedPlural = ordersCompleted != 1 ? "s" : "";

            var devicesPurchased = _gameStats.Store.PurchasedDevicesInFleaMarketCount;
            var devicesPurchasedPlural = devicesPurchased != 1 ? "s" : "";

            var devicesHacked = _gameStats.DeviceActions.HackedDevicesCount;
            var devicesHackedPlural = devicesHacked != 1 ? "s" : "";

            var moneyEarned = _gameStats.Money.EarnedMoneyAmount;

            var lastPartPurchase = _gameCalendar.CurrentDayNumber - _gameStats.Store.LastDayOfElementsPurchase;
            var lastPartPurchasePlural = lastPartPurchase != 1 ? "s" : "";

            var ratingsEarned = _ratingsService.ReviewsCount;
            var ratingsEarnedPlural = ratingsEarned != 1 ? "s" : "";

            var stickersPlaced = _gameStats.DeviceActions.AppliedStickersCount;
            var stickersPlacedPlural = stickersPlaced != 1 ? "s" : "";

            headingObj.GetComponent<TextMeshProUGUI>().SetText("Stats:");
            descriptionObj.GetComponent<TextMeshProUGUI>().SetText(
                $"{partsCleaned} Part{partsCleanedPlural} Cleaned\n"
                + $"{daysAwake} Consecutive Day{daysAwakePlural} Awake\n"
                + $"{boxesPurchased} Box{boxesPurchasedPlural} Purchased\n"
                + $"{ordersCompleted} Email Order{ordersCompletedPlural} Completed\n"
                + $"{devicesPurchased} Device{devicesPurchasedPlural} Purchased\n"
                + $"{devicesHacked} Device{devicesHackedPlural} Hacked\n"
                + $"${moneyEarned:N0} Earned\n"
                + $"{lastPartPurchase} Day{lastPartPurchasePlural} Without Buying Parts\n"
                + $"{ratingsEarned} Review{ratingsEarnedPlural} Earned\n"
                + $"{stickersPlaced} Sticker{stickersPlacedPlural} Applied\n"
            );
        }
    }
}