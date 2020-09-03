using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using SampleApp.Mobil.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace SampleApp.Mobil.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(
            INavigationService navigationService,
            IPageDialogService pageDialogService)
            : base(navigationService)
        {
            Title = "Main Page";
            _pageDialogService = pageDialogService;
        }

        private readonly IPageDialogService _pageDialogService;

        private string _uriAddress;
        /// <summary>
        /// Uri
        /// </summary>
        public string UriAddress
        {
            get { return _uriAddress; }
            set { SetProperty(ref _uriAddress, value); }
        }

        private string _to;
        /// <summary>
        /// To
        /// </summary>
        public string To
        {
            get { return _to; }
            set { SetProperty(ref _to, value); }
        }

        private string _subject;
        /// <summary>
        /// Subject
        /// </summary>
        public string Subject
        {
            get { return _subject; }
            set { SetProperty(ref _subject, value); }
        }


        private string _body;
        /// <summary>
        /// Body
        /// </summary>
        public string Body
        {
            get { return _body; }
            set { SetProperty(ref _body, value); }
        }

        public DelegateCommand SendMail => new DelegateCommand(SendMail_Executed);

        private async void SendMail_Executed()
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                    {
                        return true;
                    }
                };

                using (var httpClient = new HttpClient(clientHandler))
                {
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    EmailMessage emailMessage = new EmailMessage();
                    emailMessage.To = _to;
                    emailMessage.Subject = _subject;
                    emailMessage.Body = _body;

                    Uri requestUri = new Uri(_uriAddress);
                    var data = new StringContent(JsonConvert.SerializeObject(emailMessage),
                        Encoding.UTF8,
                        "application/json");

                    HttpResponseMessage response = await httpClient.PostAsync(requestUri, data);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var exception = JsonConvert.DeserializeObject<Exception>(responseContent);

                    if (exception == null)
                    {
                        await _pageDialogService.DisplayAlertAsync("Main Page", "Email sended.", "OK");
                    }
                    else
                    {
                        await _pageDialogService.DisplayAlertAsync("Main Page", exception.Message, "OK");
                    }
                }

            }
            catch (Exception ex)
            {
                await _pageDialogService.DisplayAlertAsync("Main Page", ex.Message, "OK");
            }
        }
    }
}
