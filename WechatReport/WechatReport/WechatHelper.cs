using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using AutoIt;

namespace WechatReport
{
    class WechatHelper
    {
        public static void Send(string groupName, string MsgContent)
        {
            //1.Đặt độ trễ của chuột xuống 50 mili giây
            //AutoItX.AutoItSetOption("MouseClickDownDelay", 50);
            //2.Trước tiên, mở cửa sổ chính của WeChat ở phía trước
            OpenMainWin();
            //3.Tìm nhóm , bạn bè trong wechat
            SearchGroup(groupName);
            //4. Gửi tin nhắn
            SendMsg(MsgContent);
        }
        private static void SendMsg(string MsgContent)
        {
            //1.Đặt con trỏ vào hộp nhập tin nhắn
            SetCursor("MSGINPUT");
            //2.Nhập nội dung tin nhắn
            PutMsgContentInto(MsgContent);
            //3.Gửi
            SendClick();
            //4.Đặt con trỏ vào hộp nhập tin nhắn
            SetCursor("MSGINPUT");
        }
        private static void PutMsgContentInto(string Msg)
        {
            //1.Xóa text
            ClearText();
            //2.Đưa nội dung vào bộ nhớ tạm
            AutoItX.ClipPut(Msg);
            //Đầu tiên chuyển đổi chuỗi thành một mảng hàng
            //string[] rows = Msg.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            //rows = rows.Select(p => p.Replace("%", "{%}").Trim()).ToArray();

            //for (int i = 0; i < rows.Length; i++)
            //{
            //    rows[i] = rows[i].Trim();
            //    SendKeys.SendWait(rows[i]);
            //    SendKeys.SendWait("^{ENTER}");
            //    AutoItX.Sleep(100);
            // }
            //3.Dán văn bản
            PasteText();

        }
        private static void PasteText()
        {
            //1.Dán tin nhắn Ctrl+V
            AutoItX.Send("^v");
        }
        private static void ClearText()
        {
            //1.Gửi Ctrl + A để chọn tất cả
            AutoItX.Send("^a");
            //2.Gửi phím nhấn để xóa
            AutoItX.Send("{BS}");
        }
        private static void GroupNameClick()
        {
            //1.Tìm tọa độ của cửa sổ chính WeChat, chiều dài và chiều rộng của cửa sổ
            Rectangle rg = AutoItX.WinGetPos("[CLASS:WeChatMainWndForPC]");
            int locationX = rg.X;
            int locationY = rg.Y;
            int width = rg.Width;
            int height = rg.Height;
            //2.Xác định vị trí của nút gửi
            int GroupNameX = locationX + (width / 6);
            int GroupNameY = locationY + 100;
            AutoItX.MouseClick("left", GroupNameX, GroupNameY);

        }
        private static void SendClick()
        {
            //1.Tìm tọa độ của cửa sổ chính WeChat, chiều dài và chiều rộng của cửa sổ
            Rectangle rg = AutoItX.WinGetPos("[CLASS:WeChatMainWndForPC]");
            int locationX = rg.X;
            int locationY = rg.Y;
            int width = rg.Width;
            int height = rg.Height;
            //2.Xác định vị trí của nút gửi
            int enterX = locationX + width - 60;
            int enterY = locationY + height - 30;
            AutoItX.MouseClick("left", enterX, enterY);
            AutoItX.Sleep(500);

        }
        private static void SearchGroup(string groupName)
        {
            //1.Đặt con trỏ trong hộp tìm kiếm
            SetCursor("SEARCHFRAME");
            //2.Nhập tên nhóm vào hộp tìm kiếm
            //AutoItX.Sleep(500);
            //3.Hộp văn bản trống
            ClearText();
            //SendKeys.SendWait(groupName);
            AutoItX.ClipPut(groupName);
            PasteText();
            AutoItX.Sleep(500);
            GroupNameClick();
            //AutoItX.Sleep(1000);
        }
        private static void SetCursor(string control)
        {
            //1.Tìm tọa độ của cửa sổ chính WeChat, chiều dài và chiều rộng của cửa sổ
            Rectangle rg = AutoItX.WinGetPos("[CLASS:WeChatMainWndForPC]");
            int locationX = rg.X;
            int locationY = rg.Y;
            int width = rg.Width;
            int height = rg.Height;
            if (control.Equals("SEARCHFRAME"))
            {
                //Xác định tọa độ vị trí của hộp tìm kiếm
                int searchX = locationX + (width / 6);
                int searchY = locationY + 30;

                //Di chuyển chuột đến hộp tìm kiếm, nhấp vào
                AutoItX.MouseClick("left", searchX, searchY);

            }
            if (control.Equals("MSGINPUT"))
            {
                //Xác định tọa độ vị trí của hộp tìm kiếm
                int MsgFrameX = locationX + (width * 2 / 3);
                int MsgFrameY = locationY + (height * 7 / 8);

                //Di chuyển chuột đến hộp tìm kiếm, nhấp vào
                AutoItX.MouseClick("left", MsgFrameX, MsgFrameY);
            }
            AutoItX.Sleep(500);
        }
        private static void OpenMainWin()
        {
            if (AutoItX.WinGetState("[CLASS:WeChatMainWndForPC]") != 15)
            {
                AutoItX.WinSetState("[CLASS:WeChatMainWndForPC]", "", AutoItX.SW_RESTORE);
            }

            AutoItX.Sleep(5000);
            //Con trỏ chuột trong hộp nhập tin nhắn
            SetCursor("MSGINPUT");
            AutoItX.Sleep(500);
        }
        public static void CloseMainWin()
        {
            if (AutoItX.WinGetState("[CLASS:WeChatMainWndForPC]") == 15)
            {
                AutoItX.WinSetState("[CLASS:WeChatMainWndForPC]", "", AutoItX.SW_HIDE);
            }
        }

    }
}
