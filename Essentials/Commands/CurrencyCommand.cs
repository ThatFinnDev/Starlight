using Starlight.Managers;

namespace Starlight.Commands;

public class CurrencyCommand : StarlightCommand
{
    internal string refID = "";
    internal string name = "";
    public override string ID => name+"s";
    public override string Usage => name + "s <amount>";
    public override CommandType type => CommandType.DontLoad | CommandType.Cheat;

    public override List<string> GetAutoComplete(int argIndex, string[] args)
    {
        if (argIndex == 0) return new List<string> { "100", "1000", "10000", "100000", "1000000", "10000000" };
        return null;
    }
    public override string Description => Tr($"cmd.currency.description", name);
    public override string ExtendedDescription
    {
        get
        {
            string key = $"cmd.currency.extendeddescription";
            string translation = Tr(key, name);
            return key == translation ? Description : translation;
        }
    }
    public override bool Execute(string[] args)
    {
        if (!args.IsBetween(1,1)) return SendUsage();
        if (!inGame) return SendLoadASaveFirst();

        int amount = 0;
        if (!TryParseInt(args[0], out amount)) return false;

        if (!CurrencyEUtil.AddCurrency(refID, amount))
            return SendError(Tr("cmd.currency.error", name));
        SendMessage(Tr("cmd.currency.success",amount, name));
        return true;
    }
}