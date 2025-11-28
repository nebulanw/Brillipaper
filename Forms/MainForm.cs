using Gh.Common.Forms;
using Gh.Common.Utilities;
using Gh.Walliant.Properties;
using Gh.Walliant.Utilities;
using Gh.Walliant.Wallpaper;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Gh.Walliant.Forms
{
  internal class MainForm : HiddenForm
  {
    private readonly IChanger changer;
    private readonly IFactory factory;
    private bool refreshing;
    private IContainer components;
    private NotifyIcon notifyIcon;
    private ContextMenuStrip mainMenuStrip;
    private ToolStripMenuItem refreshMenuItem;
    private ToolStripMenuItem saveMenuItem;
    private Timer refreshTimer;
    private ToolStripMenuItem exitMenuItem;
    private SaveFileDialog saveFileDialog;
    private BackgroundWorker refreshWorker;
    private ContextMenuStrip providerMenuStrip;
    private ToolStripMenuItem bingProviderMenuItem;
    private ToolStripMenuItem spotlightProviderMenuItem;
    private ContextMenuStrip styleMenuStrip;
    private ToolStripMenuItem centerStyleMenuItem;
    private ToolStripMenuItem tileStyleMenuItem;
    private ToolStripMenuItem stretchStyleMenuItem;
    private ToolStripMenuItem fitStyleMenuItem;
    private ToolStripMenuItem fillStyleMenuItem;
    private ToolStripMenuItem spanStyleMenuItem;
    private ContextMenuStrip settingsMenuStrip;
    private ToolStripMenuItem activeSettingsMenuItem;
    private ToolStripMenuItem providerSettingsMenuItem;
    private ToolStripMenuItem styleSettingsMenuItem;
    private ToolStripMenuItem settingsToolStripMenuItem;
    private ToolStripMenuItem launchSettingsMenuItem;
    private ToolStripMenuItem appMainMenuItem;

    public MainForm(IChanger changer, IFactory factory)
    {
      this.changer = changer;
      this.factory = factory;
      this.InitializeComponent();
      this.spanStyleMenuItem.Available = Gh.Walliant.Utilities.Host.Modern;
      this.spotlightProviderMenuItem.Available = Gh.Walliant.Utilities.Host.Supported;
    }

    protected override void OnFormCreated(EventArgs e)
    {
      base.OnFormCreated(e);
      try
      {
        this.UpdateNotifyIcon(Config.Instance.AppEnabled);
        if (!Config.Instance.AppEnabled)
          return;
        this.PerformScheduledRefresh();
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
      base.OnFormClosed(e);
      try
      {
        Config.Instance.Save();
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
    }

    private void UpdateNotifyIcon(bool approved)
    {
      this.notifyIcon.Icon = approved ? Resources.logo_on : Resources.logo_off;
    }

    private void HandleRefreshTimerTick(object sender, EventArgs e)
    {
      try
      {
        this.PerformScheduledRefresh();
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
    }

    private void HandleMainMenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
    {
      try
      {
        e.Cancel = e.CloseReason == ToolStripDropDownCloseReason.ItemClicked;
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
    }

    private void HandleMainMenuOpening(object sender, CancelEventArgs e)
    {
      bool flag2 = false;
      try
      {
        flag2 = Config.Instance.AppEnabled;
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
      finally
      {
        this.appMainMenuItem.Checked = flag2;
        this.appMainMenuItem.Enabled = true;
        if (flag2)
          this.EnableMenuItems();
        else
          this.DisableMemuItems();
      }
    }

    private void HandleRefreshMenuItemClick(object sender, EventArgs e)
    {
      try
      {
        this.PerformCustomRefresh();
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
    }

    private void PerformScheduledRefresh()
    {
      if (this.refreshing || !Config.Instance.Active)
        return;
      this.StopRefreshTimer();
      if (this.IsFresh(DateTime.Now))
        this.ScheduleNextRefresh();
      else
        this.BeginRefresh();
    }

    private void PerformCustomRefresh()
    {
      if (this.refreshing)
        return;
      this.StopRefreshTimer();
      this.BeginRefresh();
    }

    private void BeginRefresh()
    {
      try
      {
        if (!Config.Instance.AppEnabled)
          return;
        this.refreshWorker.RunWorkerAsync((object) Config.Instance.Service);
        this.DisableMemuItems();
        this.refreshing = true;
      }
      catch (Exception ex)
      {
        this.ScheduleNextRefresh();
        Logger.Instance.Error(ex);
      }
    }

    private void HandleRefreshWorkerStarted(object sender, DoWorkEventArgs e)
    {
      BackgroundWorker backgroundWorker = (BackgroundWorker) sender;
      IService service = this.factory.Create((Provider) e.Argument);
      MetaData meta = service.Locate();
      if (backgroundWorker.CancellationPending)
      {
        e.Cancel = true;
      }
      else
      {
        ImageData imageData = service.Retrieve(meta);
        if (backgroundWorker.CancellationPending)
          e.Cancel = true;
        else
          e.Result = (object) imageData;
      }
    }

    private void HandleRefreshWorkerFinished(object sender, RunWorkerCompletedEventArgs e)
    {
      try
      {
        this.EndRefresh(e);
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
    }

    private void EndRefresh(RunWorkerCompletedEventArgs e)
    {
      try
      {
        if (e.Error == null)
          this.ChangeImage(e);
        else
          Logger.Instance.Error(e.Error);
      }
      finally
      {
        this.refreshing = false;
        this.TryEnableMenuItems();
        this.ScheduleNextRefresh();
      }
    }

    private void ChangeImage(RunWorkerCompletedEventArgs e)
    {
      if (e.Cancelled)
        return;
      this.changer.Image = ((ImageData) e.Result).Image;
      Config.Instance.LastRefresh = DateTime.Now;
    }

    private void ScheduleNextRefresh()
    {
      DateTime now = DateTime.Now;
      if (this.IsFresh(now))
        this.StartRefreshTimer(Config.Instance.NextRefresh.AddMinutes(1.0).Subtract(now));
      else
        this.StartRefreshTimer(TimeSpan.FromMinutes(1.0));
    }

    private void StartRefreshTimer(TimeSpan interval)
    {
      if (!Config.Instance.Active || !Config.Instance.AppEnabled)
        return;
      this.refreshTimer.Interval = (int) interval.TotalMilliseconds;
      this.refreshTimer.Start();
    }

    private void StopRefreshTimer() => this.refreshTimer.Stop();

    private bool IsFresh(DateTime time) => Config.Instance.NextRefresh > time;

    private void DisableMemuItems()
    {
      this.providerMenuStrip.Enabled = false;
      this.styleMenuStrip.Enabled = false;
      this.activeSettingsMenuItem.Enabled = false;
      this.providerSettingsMenuItem.Enabled = false;
      this.styleSettingsMenuItem.Enabled = false;
      this.refreshMenuItem.Enabled = false;
      this.saveMenuItem.Enabled = false;
    }

    private void EnableMenuItems()
    {
      this.providerMenuStrip.Enabled = !this.refreshing;
      this.styleMenuStrip.Enabled = !this.refreshing;
      this.activeSettingsMenuItem.Enabled = !this.refreshing;
      this.providerSettingsMenuItem.Enabled = !this.refreshing;
      this.styleSettingsMenuItem.Enabled = !this.refreshing;
      this.refreshMenuItem.Enabled = !this.refreshing;
      this.saveMenuItem.Enabled = !this.refreshing;
    }

    private void TryEnableMenuItems()
    {
      try
      {
        if (!Config.Instance.AppEnabled)
          return;
        this.EnableMenuItems();
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
    }

    private void HandleSaveMenuItemClick(object sender, EventArgs e)
    {
      try
      {
        this.mainMenuStrip.Close();
        int num = (int) this.saveFileDialog.ShowDialog();
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
    }

    private void HandleSaveFileDialogFileOk(object sender, CancelEventArgs e)
    {
      try
      {
        this.SaveImage();
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
    }

    private void SaveImage()
    {
      Image image = this.changer.Image;
      using (Stream stream = this.saveFileDialog.OpenFile())
      {
        switch (this.saveFileDialog.FilterIndex)
        {
          case 1:
            image?.Save(stream, ImageFormat.Png);
            break;
          case 2:
            image?.Save(stream, ImageFormat.Jpeg);
            break;
          case 3:
            image?.Save(stream, ImageFormat.Bmp);
            break;
          case 4:
            image?.Save(stream, ImageFormat.Gif);
            break;
        }
      }
    }

    private void HandleAppMainMenuItemClick(object sender, EventArgs e)
    {
      try
      {
        bool flag = !Config.Instance.AppEnabled;
        Config.Instance.AppEnabled = flag;
        this.appMainMenuItem.Checked = flag;
        this.UpdateNotifyIcon(Config.Instance.AppEnabled);
        if (flag)
        {
          this.EnableMenuItems();
          this.PerformScheduledRefresh();
        }
        else
        {
          this.DisableMemuItems();
          this.StopRefreshTimer();
        }
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
    }

    private void HandleSettingsMenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
    {
      try
      {
        e.Cancel = e.CloseReason == ToolStripDropDownCloseReason.ItemClicked;
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
    }

    private void HandleSettingsMenuOpening(object sender, CancelEventArgs e)
    {
      try
      {
        this.CheckSettingsMenuItems();
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
    }

    private void CheckSettingsMenuItems()
    {
      this.activeSettingsMenuItem.Checked = Config.Instance.Active;
      this.launchSettingsMenuItem.Checked = Starter.Instance.Enabled;
    }

    private void HandleActiveSettingsMenuItemClick(object sender, EventArgs e)
    {
      try
      {
        this.UpdateActivity(!Config.Instance.Active);
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
    }

    private void UpdateActivity(bool active)
    {
      Config.Instance.Active = active;
      this.activeSettingsMenuItem.Checked = active;
      if (active)
        this.PerformScheduledRefresh();
      else
        this.StopRefreshTimer();
    }

    private void HandleLaunchSettingsMenuItemClick(object sender, EventArgs e)
    {
      try
      {
        this.UpdateStarter(!Starter.Instance.Enabled);
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
    }

    private void UpdateStarter(bool enabled)
    {
      Starter.Instance.Enabled = enabled;
      this.launchSettingsMenuItem.Checked = enabled;
    }

    private void HandleProviderMenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
    {
      try
      {
        e.Cancel = e.CloseReason == ToolStripDropDownCloseReason.ItemClicked;
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
    }

    private void HandleProviderMenuOpening(object sender, CancelEventArgs e)
    {
      try
      {
        this.CheckProviderMenuItem();
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
    }

    private void HandleBingProviderMenuItemClick(object sender, EventArgs e)
    {
      try
      {
        if (this.bingProviderMenuItem.Checked)
          return;
        this.UpdateProvider(Provider.Bing);
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
    }

    private void HandleSpotlightProviderMenuItemClick(object sender, EventArgs e)
    {
      try
      {
        if (this.spotlightProviderMenuItem.Checked)
          return;
        this.UpdateProvider(Provider.Spotlight);
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
    }

    private void UpdateProvider(Provider provider)
    {
      Config.Instance.Service = provider;
      this.CheckProviderMenuItem();
      this.PerformCustomRefresh();
    }

    private void CheckProviderMenuItem()
    {
      this.bingProviderMenuItem.Checked = false;
      this.spotlightProviderMenuItem.Checked = false;
      switch (Config.Instance.Service)
      {
        case Provider.Bing:
          this.bingProviderMenuItem.Checked = true;
          break;
        case Provider.Spotlight:
          this.spotlightProviderMenuItem.Checked = true;
          break;
      }
    }

    private void HandleStyleMenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
    {
      try
      {
        e.Cancel = e.CloseReason == ToolStripDropDownCloseReason.ItemClicked;
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
    }

    private void HandleStyleMenuOpening(object sender, CancelEventArgs e)
    {
      try
      {
        this.CheckStyleMenuItem();
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
    }

    private void HandleCenterStyleMenuItemClick(object sender, EventArgs e)
    {
      try
      {
        if (this.centerStyleMenuItem.Checked)
          return;
        this.UpdateStyle(Style.Center);
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
    }

    private void HandleTileStyleMenuItemClick(object sender, EventArgs e)
    {
      try
      {
        if (this.tileStyleMenuItem.Checked)
          return;
        this.UpdateStyle(Style.Tile);
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
    }

    private void HandleStretchStyleMenuItemClick(object sender, EventArgs e)
    {
      try
      {
        if (this.stretchStyleMenuItem.Checked)
          return;
        this.UpdateStyle(Style.Stretch);
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
    }

    private void HandleFitStyleMenuItemClick(object sender, EventArgs e)
    {
      try
      {
        if (this.fitStyleMenuItem.Checked)
          return;
        this.UpdateStyle(Style.Fit);
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
    }

    private void HandleFillStyleMenuItemClick(object sender, EventArgs e)
    {
      try
      {
        if (this.fillStyleMenuItem.Checked)
          return;
        this.UpdateStyle(Style.Fill);
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
    }

    private void HandleSpanStyleMenuItemClick(object sender, EventArgs e)
    {
      try
      {
        if (this.spanStyleMenuItem.Checked)
          return;
        this.UpdateStyle(Style.Span);
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
    }

    private void UpdateStyle(Style style)
    {
      this.changer.Style = style;
      this.CheckStyleMenuItem();
    }

    private void CheckStyleMenuItem()
    {
      this.centerStyleMenuItem.Checked = false;
      this.tileStyleMenuItem.Checked = false;
      this.stretchStyleMenuItem.Checked = false;
      this.fitStyleMenuItem.Checked = false;
      this.fillStyleMenuItem.Checked = false;
      this.spanStyleMenuItem.Checked = false;
      switch (this.changer.Style)
      {
        case Style.Center:
          this.centerStyleMenuItem.Checked = true;
          break;
        case Style.Tile:
          this.tileStyleMenuItem.Checked = true;
          break;
        case Style.Stretch:
          this.stretchStyleMenuItem.Checked = true;
          break;
        case Style.Fit:
          this.fitStyleMenuItem.Checked = true;
          break;
        case Style.Fill:
          this.fillStyleMenuItem.Checked = true;
          break;
        case Style.Span:
          this.spanStyleMenuItem.Checked = true;
          break;
      }
    }

    private void HandleExitMenuItemClick(object sender, EventArgs e)
    {
      try
      {
        this.mainMenuStrip.Close();
        this.refreshWorker.CancelAsync();
        this.StopRefreshTimer();
      }
      catch (Exception ex)
      {
        Logger.Instance.Error(ex);
      }
      finally
      {
        this.Close();
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MainForm));
      this.notifyIcon = new NotifyIcon();
      this.mainMenuStrip = new ContextMenuStrip();
      this.refreshMenuItem = new ToolStripMenuItem();
      this.saveMenuItem = new ToolStripMenuItem();
      this.appMainMenuItem = new ToolStripMenuItem();
      this.settingsToolStripMenuItem = new ToolStripMenuItem();
      this.settingsMenuStrip = new ContextMenuStrip();
      this.activeSettingsMenuItem = new ToolStripMenuItem();
      this.providerSettingsMenuItem = new ToolStripMenuItem();
      this.providerMenuStrip = new ContextMenuStrip();
      this.bingProviderMenuItem = new ToolStripMenuItem();
      this.spotlightProviderMenuItem = new ToolStripMenuItem();
      this.styleSettingsMenuItem = new ToolStripMenuItem();
      this.styleMenuStrip = new ContextMenuStrip();
      this.centerStyleMenuItem = new ToolStripMenuItem();
      this.tileStyleMenuItem = new ToolStripMenuItem();
      this.stretchStyleMenuItem = new ToolStripMenuItem();
      this.fitStyleMenuItem = new ToolStripMenuItem();
      this.fillStyleMenuItem = new ToolStripMenuItem();
      this.spanStyleMenuItem = new ToolStripMenuItem();
      this.launchSettingsMenuItem = new ToolStripMenuItem();
      this.exitMenuItem = new ToolStripMenuItem();
      this.refreshTimer = new Timer();
      this.saveFileDialog = new SaveFileDialog();
      this.refreshWorker = new BackgroundWorker();
      this.mainMenuStrip.SuspendLayout();
      this.settingsMenuStrip.SuspendLayout();
      this.providerMenuStrip.SuspendLayout();
      this.styleMenuStrip.SuspendLayout();
      this.SuspendLayout();
      this.notifyIcon.ContextMenuStrip = this.mainMenuStrip;
      this.notifyIcon.Icon = (Icon) componentResourceManager.GetObject("notifyIcon.Icon");
      this.notifyIcon.Text = "Walliant!";
      this.notifyIcon.Visible = true;
      this.mainMenuStrip.Items.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.refreshMenuItem,
        (ToolStripItem) this.saveMenuItem,
        (ToolStripItem) this.appMainMenuItem,
        (ToolStripItem) this.settingsToolStripMenuItem,
        (ToolStripItem) this.exitMenuItem
      });
      this.mainMenuStrip.Name = "contextMenuStrip1";
      this.mainMenuStrip.ShowCheckMargin = true;
      this.mainMenuStrip.ShowImageMargin = false;
      this.mainMenuStrip.Size = new Size(187, 202);
      this.mainMenuStrip.Closing += new ToolStripDropDownClosingEventHandler(this.HandleMainMenuClosing);
      this.mainMenuStrip.Opening += new CancelEventHandler(this.HandleMainMenuOpening);
      this.refreshMenuItem.Enabled = false;
      this.refreshMenuItem.Name = "refreshMenuItem";
      this.refreshMenuItem.Size = new Size(186, 22);
      this.refreshMenuItem.Text = "Next Wallpaper";
      this.refreshMenuItem.Click += new EventHandler(this.HandleRefreshMenuItemClick);
      this.saveMenuItem.Enabled = false;
      this.saveMenuItem.Name = "saveMenuItem";
      this.saveMenuItem.Size = new Size(186, 22);
      this.saveMenuItem.Text = "Save Wallpaper to PC";
      this.saveMenuItem.Click += new EventHandler(this.HandleSaveMenuItemClick);
      this.appMainMenuItem.Name = "appMainMenuItem";
      this.appMainMenuItem.Size = new Size(186, 22);
      this.appMainMenuItem.Text = "Enable Walliant";
      this.appMainMenuItem.Click += new EventHandler(this.HandleAppMainMenuItemClick);
      this.settingsToolStripMenuItem.DropDown = (ToolStripDropDown) this.settingsMenuStrip;
      this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
      this.settingsToolStripMenuItem.Size = new Size(186, 22);
      this.settingsToolStripMenuItem.Text = "Settings";
      this.settingsMenuStrip.Items.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.activeSettingsMenuItem,
        (ToolStripItem) this.providerSettingsMenuItem,
        (ToolStripItem) this.styleSettingsMenuItem,
        (ToolStripItem) this.launchSettingsMenuItem
      });
      this.settingsMenuStrip.Name = "settingsMenuStrip";
      this.settingsMenuStrip.OwnerItem = (ToolStripItem) this.settingsToolStripMenuItem;
      this.settingsMenuStrip.ShowCheckMargin = true;
      this.settingsMenuStrip.ShowImageMargin = false;
      this.settingsMenuStrip.Size = new Size(180, 92);
      this.settingsMenuStrip.Closing += new ToolStripDropDownClosingEventHandler(this.HandleSettingsMenuClosing);
      this.settingsMenuStrip.Opening += new CancelEventHandler(this.HandleSettingsMenuOpening);
      this.activeSettingsMenuItem.Enabled = false;
      this.activeSettingsMenuItem.Name = "activeSettingsMenuItem";
      this.activeSettingsMenuItem.Size = new Size(179, 22);
      this.activeSettingsMenuItem.Text = "Change image daily";
      this.activeSettingsMenuItem.Click += new EventHandler(this.HandleActiveSettingsMenuItemClick);
      this.providerSettingsMenuItem.DropDown = (ToolStripDropDown) this.providerMenuStrip;
      this.providerSettingsMenuItem.Enabled = false;
      this.providerSettingsMenuItem.Name = "providerSettingsMenuItem";
      this.providerSettingsMenuItem.Size = new Size(179, 22);
      this.providerSettingsMenuItem.Text = "Image source";
      this.providerMenuStrip.Enabled = false;
      this.providerMenuStrip.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.bingProviderMenuItem,
        (ToolStripItem) this.spotlightProviderMenuItem
      });
      this.providerMenuStrip.Name = "providerMenuStrip";
      this.providerMenuStrip.OwnerItem = (ToolStripItem) this.providerSettingsMenuItem;
      this.providerMenuStrip.ShowCheckMargin = true;
      this.providerMenuStrip.ShowImageMargin = false;
      this.providerMenuStrip.Size = new Size(123, 48 /*0x30*/);
      this.providerMenuStrip.Closing += new ToolStripDropDownClosingEventHandler(this.HandleProviderMenuClosing);
      this.providerMenuStrip.Opening += new CancelEventHandler(this.HandleProviderMenuOpening);
      this.bingProviderMenuItem.Enabled = false;
      this.bingProviderMenuItem.Name = "bingProviderMenuItem";
      this.bingProviderMenuItem.Size = new Size(122, 22);
      this.bingProviderMenuItem.Text = "Bing";
      this.bingProviderMenuItem.Click += new EventHandler(this.HandleBingProviderMenuItemClick);
      this.spotlightProviderMenuItem.Enabled = false;
      this.spotlightProviderMenuItem.Name = "spotlightProviderMenuItem";
      this.spotlightProviderMenuItem.Size = new Size(122, 22);
      this.spotlightProviderMenuItem.Text = "Spotlight";
      this.spotlightProviderMenuItem.Click += new EventHandler(this.HandleSpotlightProviderMenuItemClick);
      this.styleSettingsMenuItem.DropDown = (ToolStripDropDown) this.styleMenuStrip;
      this.styleSettingsMenuItem.Enabled = false;
      this.styleSettingsMenuItem.Name = "styleSettingsMenuItem";
      this.styleSettingsMenuItem.Size = new Size(179, 22);
      this.styleSettingsMenuItem.Text = "Image style";
      this.styleMenuStrip.Enabled = false;
      this.styleMenuStrip.Items.AddRange(new ToolStripItem[6]
      {
        (ToolStripItem) this.centerStyleMenuItem,
        (ToolStripItem) this.tileStyleMenuItem,
        (ToolStripItem) this.stretchStyleMenuItem,
        (ToolStripItem) this.fitStyleMenuItem,
        (ToolStripItem) this.fillStyleMenuItem,
        (ToolStripItem) this.spanStyleMenuItem
      });
      this.styleMenuStrip.Name = "styleMenuStrip";
      this.styleMenuStrip.OwnerItem = (ToolStripItem) this.styleSettingsMenuItem;
      this.styleMenuStrip.Size = new Size(112 /*0x70*/, 136);
      this.styleMenuStrip.Closing += new ToolStripDropDownClosingEventHandler(this.HandleStyleMenuClosing);
      this.styleMenuStrip.Opening += new CancelEventHandler(this.HandleStyleMenuOpening);
      this.centerStyleMenuItem.Enabled = false;
      this.centerStyleMenuItem.Name = "centerStyleMenuItem";
      this.centerStyleMenuItem.Size = new Size(111, 22);
      this.centerStyleMenuItem.Text = "Center";
      this.centerStyleMenuItem.Click += new EventHandler(this.HandleCenterStyleMenuItemClick);
      this.tileStyleMenuItem.Enabled = false;
      this.tileStyleMenuItem.Name = "tileStyleMenuItem";
      this.tileStyleMenuItem.Size = new Size(111, 22);
      this.tileStyleMenuItem.Text = "Tile";
      this.tileStyleMenuItem.Click += new EventHandler(this.HandleTileStyleMenuItemClick);
      this.stretchStyleMenuItem.Enabled = false;
      this.stretchStyleMenuItem.Name = "stretchStyleMenuItem";
      this.stretchStyleMenuItem.Size = new Size(111, 22);
      this.stretchStyleMenuItem.Text = "Stretch";
      this.stretchStyleMenuItem.Click += new EventHandler(this.HandleStretchStyleMenuItemClick);
      this.fitStyleMenuItem.Enabled = false;
      this.fitStyleMenuItem.Name = "fitStyleMenuItem";
      this.fitStyleMenuItem.Size = new Size(111, 22);
      this.fitStyleMenuItem.Text = "Fit";
      this.fitStyleMenuItem.Click += new EventHandler(this.HandleFitStyleMenuItemClick);
      this.fillStyleMenuItem.Enabled = false;
      this.fillStyleMenuItem.Name = "fillStyleMenuItem";
      this.fillStyleMenuItem.Size = new Size(111, 22);
      this.fillStyleMenuItem.Text = "Fill";
      this.fillStyleMenuItem.Click += new EventHandler(this.HandleFillStyleMenuItemClick);
      this.spanStyleMenuItem.Enabled = false;
      this.spanStyleMenuItem.Name = "spanStyleMenuItem";
      this.spanStyleMenuItem.Size = new Size(111, 22);
      this.spanStyleMenuItem.Text = "Span";
      this.spanStyleMenuItem.Click += new EventHandler(this.HandleSpanStyleMenuItemClick);
      this.launchSettingsMenuItem.Name = "launchSettingsMenuItem";
      this.launchSettingsMenuItem.Size = new Size(179, 22);
      this.launchSettingsMenuItem.Text = "Launch on startup";
      this.launchSettingsMenuItem.Click += new EventHandler(this.HandleLaunchSettingsMenuItemClick);
      this.exitMenuItem.Name = "exitMenuItem";
      this.exitMenuItem.Size = new Size(186, 22);
      this.exitMenuItem.Text = "Exit";
      this.exitMenuItem.Click += new EventHandler(this.HandleExitMenuItemClick);
      this.refreshTimer.Tick += new EventHandler(this.HandleRefreshTimerTick);
      this.saveFileDialog.FileName = "image";
      this.saveFileDialog.Filter = "PNG|*.png|JPG|*.jpg|BMP|*.bmp|GIF|*.gif";
      this.saveFileDialog.Title = "Save Wallpaper As";
      this.saveFileDialog.FileOk += new CancelEventHandler(this.HandleSaveFileDialogFileOk);
      this.refreshWorker.WorkerSupportsCancellation = true;
      this.refreshWorker.DoWork += new DoWorkEventHandler(this.HandleRefreshWorkerStarted);
      this.refreshWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.HandleRefreshWorkerFinished);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(800, 450);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (MainForm);
      this.Text = "Walliant";
      this.mainMenuStrip.ResumeLayout(false);
      this.settingsMenuStrip.ResumeLayout(false);
      this.providerMenuStrip.ResumeLayout(false);
      this.styleMenuStrip.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
