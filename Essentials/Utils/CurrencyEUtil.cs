using Il2CppMonomiPark.SlimeRancher.Economy;

namespace Starlight.Utils;

public static class CurrencyEUtil
{
    public static ICurrency ToICurrency(this CurrencyDefinition currencyDefinition) => currencyDefinition.TryCast<ICurrency>();
    public static CurrencyDefinition ToCurrency(this ICurrency iCurrency) => iCurrency.TryCast<CurrencyDefinition>();
    public static bool SetCurrency(string referenceID, int amount)
    {
        if (string.IsNullOrWhiteSpace(referenceID)) return false;
        if (!inGame) return false;
        var id = referenceID;
        if (!id.StartsWith("CurrencyDefinition.")) id = "CurrencyDefinition." + id;

        var def = gameContext.LookupDirector.CurrencyList.FindCurrencyByReferenceId(id);
        sceneContext.PlayerState._model.SetCurrency(def.ToICurrency(), amount);
        return true;
    }

    public static bool SetCurrency(string referenceID, int amount, int amountEverCollected)
    {
        if (string.IsNullOrWhiteSpace(referenceID)) return false;
        if (!inGame) return false;
        var id = referenceID;
        if (!id.StartsWith("CurrencyDefinition.")) id = "CurrencyDefinition." + id;

        var def = gameContext.LookupDirector.CurrencyList.FindCurrencyByReferenceId(id);
        sceneContext.PlayerState._model.SetCurrencyAndAmountEverCollected(def.ToICurrency(), amount,
            amountEverCollected);
        return true;
    }

    public static bool SetCurrencyEverCollected(string referenceID, int amountEverCollected)
    {
        if (string.IsNullOrWhiteSpace(referenceID)) return false;
        if (!inGame) return false;
        var id = referenceID;
        if (!id.StartsWith("CurrencyDefinition.")) id = "CurrencyDefinition." + id;

        var def = gameContext.LookupDirector.CurrencyList.FindCurrencyByReferenceId(id);
        sceneContext.PlayerState._model.SetCurrencyAndAmountEverCollected(def.ToICurrency(),
            GetCurrency(referenceID), amountEverCollected);
        return true;
    }

    public static bool AddCurrency(string referenceID, int amount)
    {
        if (string.IsNullOrWhiteSpace(referenceID)) return false;
        if (!inGame) return false;
        var id = referenceID;
        if (!id.StartsWith("CurrencyDefinition.")) id = "CurrencyDefinition." + id;

        var def = gameContext.LookupDirector.CurrencyList.FindCurrencyByReferenceId(id);
        sceneContext.PlayerState._model.AddCurrency(def.ToICurrency(), amount);
        return true;
    }

    public static int GetCurrency(string referenceID)
    {
        if (string.IsNullOrWhiteSpace(referenceID)) return -1;
        if (!inGame) return -1;
        var id = referenceID;
        if (!id.StartsWith("CurrencyDefinition.")) id = "CurrencyDefinition." + id;

        var def = gameContext.LookupDirector.CurrencyList.FindCurrencyByReferenceId(id);
        var curr = sceneContext.PlayerState._model.GetCurrencyAmount(def.ToICurrency());
        if (curr.ToString() == "NaN") return 0;
        return curr;
    }

    public static int GetCurrencyEverCollected(string referenceID)
    {
        if (string.IsNullOrWhiteSpace(referenceID)) return -1;
        if (!inGame) return -1;
        var id = referenceID;
        if (!id.StartsWith("CurrencyDefinition.")) id = "CurrencyDefinition." + id;

        var def = gameContext.LookupDirector.CurrencyList.FindCurrencyByReferenceId(id);
        var curr = sceneContext.PlayerState._model.GetCurrencyAmountEverCollected(def.ToICurrency());
        if (curr.ToString() == "NaN") return 0;
        return curr;
    }
}