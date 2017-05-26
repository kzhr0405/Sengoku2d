using System.Collections;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine.UI;

public class PvPTimer : MonoBehaviour {

    private DateTime ntpDate = DateTime.MinValue;   // NTP同期時刻
    private float rcvAppDate;   // NTP通信時のアプリ時刻

    private IPEndPoint ipAny;
    private UdpClient sock;
    private Thread thread;
    private volatile bool threadRunning = false;
    private byte[] rcvData;

    //pvp timer
    public double timer = 0;
    public string start = "";
    public string now = "";
    public string end = "";
    public string startNCMB = ""; //yyyymmdd
    public string endNCMB = ""; //yyyymmdd
    public string todayNCMB = ""; //yyyymmdd

    Text timerTxt;
    public bool engFlg = false;
    private bool resetFlag = true;
    
    void Awake() {
        Init();

        timerTxt = GameObject.Find("Timer").GetComponent<Text>();
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            engFlg = true;
        }
    }

    // 初期化
    void Init()
    {
        // リクエスト実行
        SyncDate();

        // 時刻表示(デバッグ用)
        StartCoroutine(ShowSyncDate());
    }

    // 同期時刻の表示
    private IEnumerator ShowSyncDate() {
        while (true) {
            yield return new WaitForSeconds(1);

            if (Date == DateTime.MinValue) {
                Debug.Log("Time is not received.");
            }else {
                DateTime sunday = GetNearestDayOfWeek(Date, DayOfWeek.Sunday);
                DateTime monday = GetNearestMonday(Date);
                sunday = DateTime.Parse(sunday.ToShortDateString().Trim() + " 23:59:59");
                monday = DateTime.Parse(monday.ToShortDateString().Trim() + " 00:00:00");
                TimeSpan diff = sunday - Date;
                timer = diff.TotalSeconds;
                now = Date.ToString();
                end = sunday.ToString();
                start = monday.ToString();

                startNCMB = monday.ToString("yyyyMMdd");
                endNCMB = sunday.ToString("yyyyMMdd");
                todayNCMB = Date.ToString("yyyyMMdd");

                resetFlag = false;
                yield break;
            }
        }
    }

    // アプリケーション終了時処理
    void OnApplicationQuit() {
        if (thread != null) {
            thread.Abort();
        }
        if (sock != null) {
            sock.Close();
        }
    }

    // 時刻同期を行う
    public void SyncDate() {
        // リクエスト実行
        threadRunning = true;
        thread = new Thread(new ThreadStart(Request));
        thread.Start();

        // リクエスト待機コルーチン実行
        StartCoroutine(WaitForRequest());

        Debug.Log("Thread is started.");
    }

    // NTPサーバに対してリクエストを実行する
    private void Request() {
        // ソケットを開く
        ipAny = new IPEndPoint(IPAddress.Any, 123);
        sock = new UdpClient();

        // リクエスト送信
        byte[] sndData = new byte[48];
        sndData[0] = 0xB;
        sock.Send(sndData, sndData.Length, "ntp.jst.mfeed.ad.jp", 123);

        // データ受信
        rcvData = sock.Receive(ref ipAny);

        // 実行中フラグクリア
        threadRunning = false;
    }

    // リクエスト待機コルーチン
    private IEnumerator WaitForRequest() {
        // リクエスト終了まで待機
        while (threadRunning) {
            yield return 0;
        }

        // アプリ時刻保存
        rcvAppDate = Time.realtimeSinceStartup;

        // 受信したバイナリデータをDateTime型に変換
        ntpDate = new DateTime(1900, 1, 1);
        var high = (double)BitConverter.ToUInt32(new byte[] { rcvData[43], rcvData[42], rcvData[41], rcvData[40] }, 0);
        var low = (double)BitConverter.ToUInt32(new byte[] { rcvData[47], rcvData[46], rcvData[45], rcvData[44] }, 0);
        ntpDate = ntpDate.AddSeconds(high + low / UInt32.MaxValue);

        // UTC→ローカル日時に変換
        ntpDate = ntpDate.ToLocalTime();
    }

    // NTP同期時刻
    public DateTime Date {
        get {
            if (ntpDate == DateTime.MinValue)
                return DateTime.MinValue;
            else
                return ntpDate.AddSeconds(Time.realtimeSinceStartup - rcvAppDate);
        }
    }

    //指定日から最も近い未来の日曜日を返す
    public static DateTime GetNearestDayOfWeek(DateTime day, DayOfWeek wantDayOfWeek) {
        DayOfWeek dayOfWeek = day.DayOfWeek;

        if (dayOfWeek == wantDayOfWeek) {
            return day;
        }else {
            return day.AddDays(
                ((int)(DayOfWeek.Saturday - day.DayOfWeek + wantDayOfWeek) % 7) + 1);
        }
    }

    //指定日からさかのぼり最も近い過去の月曜を返す
    public static DateTime GetNearestMonday(DateTime day) {
        int diff = DayOfWeek.Monday - day.DayOfWeek;
        if (diff > 0)
            diff -= 7;
        return day.AddDays(diff);
    }



    void Update() {
        
        timer -= Time.deltaTime;

        if (timer > 0.0f) {
            //On Play
            TimeSpan time = TimeSpan.FromSeconds(timer);
            string displayTime = "";
            if (engFlg) {
                displayTime = time.Days + "d " + time.Hours + "h " + time.Minutes + "m " + time.Seconds + "s";
            }else {
                displayTime = time.Days + "日 " + time.Hours + "時間 " + time.Minutes + "分 " + time.Seconds + "秒";
            }
            timerTxt.text = displayTime;

        }else {
            //reset
            if (!resetFlag)
            {
                resetFlag = true;
                Init();
            }
        }

    }
    
}
