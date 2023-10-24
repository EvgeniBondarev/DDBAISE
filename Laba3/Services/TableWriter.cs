namespace Laba3.Services
{
    public class TableWriter
    {
        public string WriteTable(IEnumerable<Subscription> subscriptions, params object[] addons)
        {
            string HtmlString = "<HTML><HEAD><TITLE>Подписки</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>";
            foreach (string addon in addons)
            {
                HtmlString += addon;
            } 
            HtmlString +="<BODY><H1>Список подписок</H1><TABLE BORDER=1>";
            HtmlString += "<TR>";
            HtmlString += "<TH>ID</TH>";
            HtmlString += "<TH>Продолжительность</TH>";
            HtmlString += "<TH>Дата подписки</TH>";
            HtmlString += "<TH>Получатель</TH>";
            HtmlString += "<TH>Издание</TH>";
            HtmlString += "<TH>Офис</TH>";
            HtmlString += "</TR>";
            foreach (var subscription in subscriptions)
            {
                HtmlString += "<TR>";
                HtmlString += "<TD>" + subscription.Id + "</TD>";
                HtmlString += "<TD>" + subscription.Duration + "</TD>";
                HtmlString += "<TD>" + subscription.SubscriptionStartDate + "</TD>";
                HtmlString += "<TD>" + subscription.Recipient.ToString() + "</TD>";
                HtmlString += "<TD>" + subscription.Publication.Name + "</TD>";
                HtmlString += "<TD>" + subscription.Office.StreetName + "</TD>";
                HtmlString += "</TR>";
            }
            HtmlString += "</TABLE>";
            HtmlString += "<BR><A href='/'>Главная</A></BR>";
            HtmlString += "</BODY></HTML>";

            return HtmlString;
        }

    }
}
