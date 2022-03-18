using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Loader_WPF.Core;
using Loader_WPF.Core.Json;
using Loader_WPF.Core.Cheat;
using Loader_WPF.Core.Inject;
using System.Threading;

namespace Loader_WPF
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        private User user = new User();
        private AllEnums.Forms currentForm = AllEnums.Forms.Login;
        private InjectionCheat injectionData = new InjectionCheat();

        public MainWindow()
        {
            InitializeComponent();

            SetComponent();
        }

        void SetComponent()
        {
            SetGrid(AllEnums.Forms.Login);

            _boxInjection_MsgTop.Text = "\nVerificando integridade, aguarde...";
            _boxValidation_Text.Text = "Para continuar é necessario fazer uma recarga.\nClique no botão fazer recarga, depois que seguir\ntodos os passos, clique em validar.";

            if (File.Exists(@"save.txt"))
            {
                txt_username.Text = File.ReadAllText(@"save.txt");
                _login_checkbox.IsChecked = true;
            }
        }

        void CheatsList(List<CheatsJson> list)
        {
            try
            {
                _boxInjection_cheat_list.Items.Clear();

                IEnumerable<CheatsJson> query = from ls in list orderby ls.Status descending select ls;

                foreach (var item in query)
                {
                    _boxInjection_cheat_list.Items.Add(item);
                    injectionData.AddItem(item);
                }

                //Form Injection
                SetGrid(AllEnums.Forms.Injection);
                currentForm = AllEnums.Forms.Injection;
            }
            catch
            {
                MsgError(1, "Error #3", 1);
            }
        }

        public void SetGrid(AllEnums.Forms form)
        {
            switch (form)
            {
                case AllEnums.Forms.Error:
                    _boxLogin.Visibility = Visibility.Hidden;
                    _boxValidation.Visibility = Visibility.Hidden;
                    _boxInjection.Visibility = Visibility.Hidden;
                    _ErrorMsg.Visibility = Visibility.Visible;
                    break;
                case AllEnums.Forms.Login:
                    _ErrorMsg.Visibility = Visibility.Hidden;
                    _boxValidation.Visibility = Visibility.Hidden;
                    _boxInjection.Visibility = Visibility.Hidden;
                    _boxLogin.Visibility = Visibility.Visible;
                    break;
                case AllEnums.Forms.Recharge:
                    _boxLogin.Visibility = Visibility.Hidden;
                    _ErrorMsg.Visibility = Visibility.Hidden;
                    _boxInjection.Visibility = Visibility.Hidden;
                    _boxValidation.Visibility = Visibility.Visible;
                    break;
                case AllEnums.Forms.Injection:
                    _ErrorMsg.Visibility = Visibility.Hidden;
                    _boxValidation.Visibility = Visibility.Hidden;
                    _boxLogin.Visibility = Visibility.Hidden;
                    _boxInjection.Visibility = Visibility.Visible;
                    break;
                default:
                    break;

            }
        }

        async void MsgError(int type, string msg, int other = 0)
        {
            SetGrid(AllEnums.Forms.Error);

            if (type == 1)
            {
                _ErrorMsg_Background.Fill = new ImageBrush
                {
                    ImageSource = ResourceManager.GetImageFromResource("images/panel-error.png")
                };

                _ErrorMsg_Alert.Fill = new ImageBrush
                {
                    ImageSource = ResourceManager.GetImageFromResource("images/icon-error.png")
                };
            }
            else
            {
                _ErrorMsg_Background.Fill = new ImageBrush
                {
                    ImageSource = ResourceManager.GetImageFromResource("images/panel-sucess.png")
                };

                _ErrorMsg_Alert.Fill = new ImageBrush
                {
                    ImageSource = ResourceManager.GetImageFromResource("images/sucess-icon.png")
                };
            }

            _ErrorMsg_Text.FontSize = 12;
            _ErrorMsg_Text.Text = msg;

            await Task.Delay(1500);

            if (other == 0)
                SetGrid(currentForm);
        }

        #region Buttons

        private void btn_login_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (txt_username.Text == "" || txt_password.Password == "")
                {
                    MsgError(1, "Complete todos os campos.");
                    return;
                }

                AllEnums.ResultEnum resp = Core.Auth.Login.SendLogin(txt_username.Text, txt_password.Password, out user);

                user.username = txt_username.Text;
                user.password = txt_password.Password;

                if (resp != Core.AllEnums.ResultEnum.success)
                {
                    MsgError(1, user.message);
                }
                else
                {
                    Core.AllEnums.Forms nextForm = user.recharge == 0 ? AllEnums.Forms.Injection : AllEnums.Forms.Recharge;

                    if (nextForm == AllEnums.Forms.Injection)
                    {
                        SetGrid(nextForm);
                        currentForm = nextForm;

                        StartDownloadInjectorDelay();
                    }
                }
            }
            catch
            {
                MsgError(1, "0xD0007: Entre em contato com o suporte.", 1); // falha baixar injetor como admin
            }
        }

        private void btn_recharge_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("");
        }

        private void btn_valid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                AllEnums.ResultEnum resp = Core.Auth.Login.SendLogin(user.username, user.password, out user);

                if (resp != AllEnums.ResultEnum.success)
                    MsgError(1, user.message);
                else
                {
                    SetGrid(AllEnums.Forms.Injection);
                    CheatsList(CheatList.returnList(user.cheats));
                }
            }
            catch
            {
                MsgError(1, "Error #2", 1);
            }
        }

        private void btn_close_application_MouseDown(object sender, MouseButtonEventArgs e)
        {
            App.Current.MainWindow.Close();
        }

        private void btn_inject_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InjectDLL();
        }

        #endregion

        #region Others
        private void MoveForm(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void txt_username_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_login_checkbox.IsChecked == true)
                File.WriteAllText(@"save.txt", txt_username.Text);
        }

        private void _login_checkbox_Click(object sender, RoutedEventArgs e)
        {
            if (_login_checkbox.IsChecked == false)
                File.Delete(@"save.txt");
            else
                File.WriteAllText(@"save.txt", txt_username.Text);
        }

        private void _selected_cheat(object sender, SelectionChangedEventArgs e)
        {
            CheatsJson item = (CheatsJson)e.AddedItems[0];
            injectionData.SetCurrentCheat(item.id);
        }

        private async void InjectDLL()
        {
            MsgError(2, "Aguarde...", 1);

            try
            {
                CheatsJson Cheat = injectionData.returnCurrentCheatData();

                if (Cheat != null)
                {
                    string dll_path = Core.Inject.DownloadDLL.DllName(Cheat.path_dll).Result;
                    string process_name = Core.Inject.CheatManager.HandleProcessName(Cheat.Game, Cheat.process_name);

                    await Task.Delay(1000);

                    if (dll_path != null)
                    {
                        Process injection = Core.Inject.CheatManager.RunInjector(process_name, dll_path);
                        MsgError(2, string.Format("Cheat carregado.\nAguardando \"{0}\"'.", Cheat.Game), 1);

                        while (!injection.HasExited)
                        {
                            await Task.Delay(1000);
                        }

                        Core.Inject.CheatManager.CamouflagedDll(dll_path);
                        this.Close();
                    }
                    else
                    {
                        MsgError(1, "0xC0006: Entre em contato com o suporte.", 1); // falha baixar dll
                    }
                }
                else
                {
                    MsgError(1, "0xF0015: Cheat indisponível."); // cheat off
                }
            }
            catch
            {
                MsgError(1, "0xD0007: Entre em contato com o suporte.", 1); // perm admin
            }
        }
        #endregion

        private void _boxInjection_progressbar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private async void StartDownloadInjectorDelay()
        {
            await Task.Delay(1000);

            Task b = DownloadDLL.DownloadInjector();

            DownloadDLL.action = new System.Action(() => {
                _boxInjection_progressbar.Value = DownloadDLL.progressDownload;

                if (DownloadDLL.progressDownload == 100)
                {
                    _boxInjection_MsgTop_Icon.Fill = new ImageBrush { ImageSource = ResourceManager.GetImageFromResource("images/icon-error.png") };
                    _boxInjection_MsgTop.Text = "\nPara continuar você deve escolher o jogo que deseja\niniciar o cheat.";
                }
            });

            if (b.IsCompleted)
                CheatsList(CheatList.returnList(user.cheats));
        }
    }
}
