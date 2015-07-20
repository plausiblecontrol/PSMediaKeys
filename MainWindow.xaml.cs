using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Keyz {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

    [DllImport("user32.dll")]
    extern static uint SendMessage(IntPtr hWnd, uint msg, IntPtr wParam,
      IntPtr lParam);

    [DllImport("user32.dll", EntryPoint = "PostMessageA")]
    static extern bool PostMessage(IntPtr hWnd, uint msg, uint wParam, int lParam);

    [DllImport("user32.dll")]
    static extern byte VkKeyScan(char ch);

    [DllImport("User32.dll")]
    private static extern bool RegisterHotKey(
        [In] IntPtr hWnd,
        [In] int id,
        [In] uint fsModifiers,
        [In] uint vk);

    [DllImport("User32.dll")]
    private static extern bool UnregisterHotKey(
        [In] IntPtr hWnd,
        [In] int id);

    private HwndSource _source;
    private const int HOTKEY_ID = 9000;

    const uint MOD_CTRL = 0x0002;
    const uint WM_KEYDOWN = 0x100;
    const uint WM_KEYUP = 0x101;
    const uint VK_RIGHT = 0x27;
    const uint VK_LEFT = 0x25;
    const int K_LEFT = 2424834;
    const int K_RIGHT = 2555906;
    const uint VK_SPACE = 0x20;
    const int K_SPACE = 2097154;
    const uint VK_SUBTRACT = 0x6D;
    const int K_SUBTRACT = 7143426;
    const uint VK_ADD = 0x6B;
    const int K_ADD = 7012354;
    const uint VK_CONTROL = 0x11; //ctrl
    const uint VK_MENU = 0x12; //alt
    const uint VK_UP = 0x26;
    const int K_UP = 2490370;
    const uint VK_DOWN = 0x28;
    const int K_DOWN = 2621442;
    const uint VK_NUMPAD0 = 0x60;
    const uint VK_MEDIA_PLAY_PAUSE = 0xB3;
    //const int mediakk = (int)VK_MEDIA_PLAY_PAUSE;
    const uint VK_MEDIA_PREV_TRACK = 0xB1;
    const uint VK_MEDIA_NEXT_TRACK = 0xB0;
    const uint VK_MEDIA_STOP = 0xB2;

    public MainWindow() {
      InitializeComponent();
    }

    protected override void OnSourceInitialized(EventArgs e) {
      base.OnSourceInitialized(e);
      var helper = new WindowInteropHelper(this);
      _source = HwndSource.FromHwnd(helper.Handle);
      _source.AddHook(HwndHook);
      RegisterHotKeys();
    }

    protected override void OnClosed(EventArgs e) {
      _source.RemoveHook(HwndHook);
      _source = null;
      UnregisterHotKey();
      base.OnClosed(e);
    }

    private void RegisterHotKeys() {
      var helper = new WindowInteropHelper(this);
      if (!RegisterHotKey(helper.Handle, HOTKEY_ID, MOD_CTRL, VK_SPACE)) {
        // handle error
      }
      if (!RegisterHotKey(helper.Handle, HOTKEY_ID, MOD_CTRL, VK_RIGHT)) {
        // handle error
      }
      if (!RegisterHotKey(helper.Handle, HOTKEY_ID, MOD_CTRL, VK_UP)) {
        // handle error
      }
      if (!RegisterHotKey(helper.Handle, HOTKEY_ID, MOD_CTRL, VK_DOWN)) {
        // handle error
      }
      if (!RegisterHotKey(helper.Handle, HOTKEY_ID, MOD_CTRL, VK_SUBTRACT)) {
        // handle error
      }
      if (!RegisterHotKey(helper.Handle, HOTKEY_ID, MOD_CTRL, VK_ADD)) {
        // handle error
      }
      if (!RegisterHotKey(helper.Handle, HOTKEY_ID, MOD_CTRL, VK_LEFT)) {
        // handle error
      }
      if (!RegisterHotKey(helper.Handle, HOTKEY_ID, MOD_CTRL, VK_NUMPAD0)) {
        // handle error
      }
    }

    private void UnregisterHotKey() {
      var helper = new WindowInteropHelper(this);
      UnregisterHotKey(helper.Handle, HOTKEY_ID);
    }

    private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
      const int WM_HOTKEY = 0x0312;
      switch (msg) {
        case WM_HOTKEY:
          int n = lParam.ToInt32();
          switch (n) {
            case K_SPACE:
              SendPause();
              handled = true;
              break;
            case K_RIGHT:
              SendSkip();
              handled = true;
              break;
            case 6291458:
              SendMute();
              handled = true;
              break;
            case K_LEFT:
              SendBack();
              handled = true;
              break;
            case K_SUBTRACT:
              SendDown();
              handled = true;
              break;
            case K_ADD:
              SendUp();
              handled = true;
              break;
            case K_UP:
              SendVUp();
              handled = true;
              break;
            case K_DOWN:
              SendVDown();
              handled = true;
              break;
          }
          break;
      }
      return IntPtr.Zero;
    }

    public static Process[] getProcs() {
      //Process[] chrome = Process.GetProcessesByName("chrome");
      Process[] pandora = Process.GetProcessesByName("Pandora");
      Process[] spotify = Process.GetProcessesByName("spotify");
      if (pandora.Count() > 0) {
        return pandora;
      } else if (spotify.Count() > 0) {
        return spotify;
      } else {
        return null;
      }
    }

    static void SendSkip() {
      Process[] procs = getProcs();
      foreach (Process proc in procs) {
        IntPtr HHh = proc.MainWindowHandle;
        if (proc.MainWindowTitle == "Pandora") {          
          PostMessage(HHh, WM_KEYDOWN, VK_RIGHT, 0);
          PostMessage(HHh, WM_KEYUP, VK_RIGHT, 0);
        } else {
          SendMessage(HHh, 0x0319, IntPtr.Zero, new IntPtr((long)720896));
        }
      }
    }

    static void SendMute() {
      Process[] procs = getProcs();
      foreach (Process proc in procs) {
        IntPtr HHh = proc.MainWindowHandle;
          SendMessage(HHh, 0x0319, IntPtr.Zero, new IntPtr((long)524288)); 
      }
    }
    static void SendBack() {
      Process[] procs = getProcs();
      foreach (Process proc in procs) {
        IntPtr HHh = proc.MainWindowHandle;
        if (proc.MainWindowTitle != "Pandora") {
        SendMessage(HHh, 0x0319, IntPtr.Zero, new IntPtr((long)786432));
        }
      }
    }
    static void SendPause() {
      Process[] procs = getProcs();
      if (procs.Count() > 0) {
        foreach (Process proc in procs) {
          IntPtr HHh = proc.MainWindowHandle;
          if (proc.MainWindowTitle == "Pandora") {
            PostMessage(HHh, WM_KEYDOWN, VK_SPACE, 0);
            PostMessage(HHh, WM_KEYUP, VK_SPACE, 0);
          } else {
            SendMessage(HHh, 0x0319, IntPtr.Zero, new IntPtr((long)917504));
          }
          
        }
      }
    }
    static void SendUp() {
      Process[] procs = getProcs();
      if (procs.Count() > 0) {
        foreach (Process proc in procs) {
          IntPtr HHh = proc.MainWindowHandle;
          if (proc.MainWindowTitle == "Pandora") {
            PostMessage(HHh, WM_KEYDOWN, VK_ADD, 0);
            PostMessage(HHh, WM_KEYUP, VK_ADD, 0);
          } 
          //else {
          //  SendMessage(HHh, 0x0319, IntPtr.Zero, new IntPtr((long)655360)); //system volume up
          //}
        }
      }
    }
    static void SendDown() {
      Process[] procs = getProcs();
      if (procs.Count() > 0) {
        foreach (Process proc in procs) {
          IntPtr HHh = proc.MainWindowHandle;
          if (proc.MainWindowTitle == "Pandora") {
            PostMessage(HHh, WM_KEYDOWN, VK_SUBTRACT, 0);
            PostMessage(HHh, WM_KEYUP, VK_SUBTRACT, 0);
          } 
          //else {
          //  SendMessage(HHh, 0x0319, IntPtr.Zero, new IntPtr((long)589824)); //system volume down
          //}          
        }
      }
    }
    static void SendVUp() {
      Process[] procs = getProcs();
      if (procs.Count() > 0) {
        foreach (Process proc in procs) {
          IntPtr HHh = proc.MainWindowHandle;
          //SendMessage(HHh, 0x0319, IntPtr.Zero, new IntPtr((long)655360));
          Thread.Sleep(200);
          PostMessage(HHh, WM_KEYDOWN, VK_UP, 0x1F0001);
          PostMessage(HHh, WM_KEYUP, VK_UP, 0x1F0001);
        }
      }
    }
    static void SendVDown() {
      Process[] procs = getProcs();
      if (procs.Count() > 0) {
        foreach (Process proc in procs) {
          IntPtr HHh = proc.MainWindowHandle;
          //SendMessage(HHh, 0x0319, IntPtr.Zero, new IntPtr((long)589824));
          Thread.Sleep(200);
          PostMessage(HHh, WM_KEYDOWN, VK_DOWN, 0);
          PostMessage(HHh, WM_KEYUP, VK_DOWN, 0);
        }
      }
    }

  }
}
