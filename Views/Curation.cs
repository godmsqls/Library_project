using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LibraryProject.Models;
using LibraryProject.Services;

namespace LibraryProject.Views
{
    public class Curation : Form
    {
        private FlowLayoutPanel flowRecommended;
        private Panel pnlChartBase;

        public Curation()
        {
            this.Text = "맞춤 도서 추천";
            this.Size = new Size(720, 640);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Font = new Font("Noto Sans KR", 9F);

            Label lblChart = new Label 
            { 
                Text = "대출 통계", 
                Location = new Point(30, 20), 
                AutoSize = true, 
                Font = new Font("Noto Sans KR", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(51,51,51)
            };

            pnlChartBase = new Panel 
            { 
                Location = new Point(30, 50), 
                Size = new Size(620, 200), 
                BorderStyle = BorderStyle.None, 
                BackColor = Color.FromArgb(248,249,250) 
            };
            pnlChartBase.Paint += PnlChartBase_Paint;

            Label lblRecommend = new Label 
            { 
                Text = "AI 추천 도서", 
                Location = new Point(30, 270), 
                AutoSize = true,
                Font = new Font("Noto Sans KR", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51)
            };

            flowRecommended = new FlowLayoutPanel 
            { 
                Location = new Point(30, 300), 
                Size = new Size(640, 280), 
                AutoScroll = true 
            };

            this.Controls.AddRange(new Control[] { lblChart, pnlChartBase, lblRecommend, flowRecommended });
        }

        private List<LoanRecord> _history = new List<LoanRecord>();

        public void DisplayStatistics(List<LoanRecord> history)
        {
            _history = history;
            pnlChartBase.Invalidate();
        }

        private void PnlChartBase_Paint(object sender, PaintEventArgs e)
        {
            if (_history == null || _history.Count == 0)
            {
                e.Graphics.DrawString("대출 이력이 없습니다.", new Font("Noto Sans KR", 10), Brushes.Black, new PointF(10, 10));
                return;
            }

            var categoryCounts = _history
                .Where(h => !string.IsNullOrEmpty(h.CategoryName))
                .GroupBy(h => 
                {
                    var parts = h.CategoryName.Split('>');
                    return parts.Length > 1 ? parts[1].Trim() : parts[0].Trim();
                })
                .ToDictionary(g => g.Key, g => g.Count());

            if (categoryCounts.Count == 0) return;

            int maxCount = categoryCounts.Values.Max();
            int margin = 25;
            int barHeight = Math.Max(22, (pnlChartBase.Height - margin * 2) / categoryCounts.Count - 8);
            int currentY = margin;
            int maxBarWidth = pnlChartBase.Width - 280;

            foreach (var kvp in categoryCounts)
            {
                int barWidth = maxCount == 0 ? 0 : (int)((double)kvp.Value / maxCount * maxBarWidth);
                
                e.Graphics.FillRectangle(Brushes.SkyBlue, 150, currentY, barWidth, barHeight);
                e.Graphics.DrawRectangle(Pens.Black, 150, currentY, barWidth, barHeight);
                
                e.Graphics.DrawString(kvp.Key, new Font("Noto Sans KR", 9), Brushes.Black, new RectangleF(10, currentY, 130, barHeight), new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(kvp.Value.ToString() + "권", new Font("Noto Sans KR", 9), Brushes.Black, new PointF(150 + barWidth + 5, currentY + barHeight / 2 - 7));
                
                currentY += barHeight + 8;
            }
        }

        public void DisplayRecommendations(List<BookItem> books)
        {
            flowRecommended.Controls.Clear();
            foreach (var book in books)
            {
                Button btnBook = new Button
                {
                    Text = $"{book.Title}\n({book.Author})",
                    Size = new Size(180, 100),
                    Margin = new Padding(10),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                flowRecommended.Controls.Add(btnBook);
            }
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
