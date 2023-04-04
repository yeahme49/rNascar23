﻿using rNascar23.Points.Models;
using rNascar23.CustomViews;
using rNascar23.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rNascar23.Views
{
    public partial class StagePointsGridView : UserControl, IGridView<Stage>
    {
        #region consts

        private const int CautionFlag = 2;

        #endregion

        #region properties

        public ApiSources ApiSource => ApiSources.StagePoints;
        public string Title => "Stage Points";

        private IList<Stage> _data = new List<Stage>();
        public IList<Stage> Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;


                if (_data != null)
                {
                    var stage = _data.FirstOrDefault();

                    if (stage != null)
                        SetDataSource(stage.StagePoints);
                }
            }
        }
        public string CustomGridName { get; set; }
        public string Description { get; set; }
        public GridSettings Settings { get; set; }
        public bool IsCustomGrid { get; set; }
        public DataGridView DataGridView
        {
            get
            {
                return Grid;
            }
        }

        #endregion

        #region ctor

        public StagePointsGridView()
        {
            InitializeComponent();

            BuildGridView(Grid);

            Settings = new GridSettings()
            {
                ApiSource = ApiSources.StagePoints,
                SortOrderField = "Position",
                SortOrder = 1
            };
        }

        #endregion

        #region private

        private void SetDataSource<T>(IList<T> values)
        {
            var models = BuildViewModels((IList<StagePoints>)values);

            var dataTable = GridViewTableBuilder.ToDataTable<DriverPointsViewModel>(models.ToList());

            var dataView = new DataView(dataTable);

            GridBindingSource.DataSource = dataView;

            Grid.DataSource = GridBindingSource;

            foreach (DataGridViewTextBoxColumn column in Grid.Columns)
            {
                column.Name = column.DataPropertyName;
            }

            if (!String.IsNullOrEmpty(Settings.SortOrderField))
            {
                var sortDirection = Settings.SortOrder == 0 ? String.Empty :
                    Settings.SortOrder == 1 ? "ASC" :
                    "DESC";

                var sortString = $"{Settings.SortOrderField} {sortDirection}".Trim();

                GridBindingSource.Sort = sortString;
            }
            var stage = Data.FirstOrDefault();

            if (stage != null && stage.stage_number != 0)
            {
                TitleLabel.Text = $"Stage Points (Stage {stage.stage_number})";
            }
            else
            {
                TitleLabel.Text = "Stage Points";
            }
        }

        private IList<DriverPointsViewModel> BuildViewModels(IList<StagePoints> data)
        {
            return data.
                 Select(p => new DriverPointsViewModel()
                 {
                     Position = p.position,
                     Driver = p.full_name,
                     Points = p.stage_points
                 }).OrderBy(p => p.Position).ToList();
        }

        private DataGridView BuildGridView(DataGridView dataGridView)
        {
            DataGridViewTextBoxColumn Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();

            dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                Column1,
                Column2,
                Column3,
            });

            dataGridView.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Regular);

            GridViewColumnBuilder.ConfigureColumn(Column1, "Position", 25, "");

            GridViewColumnBuilder.ConfigureColumn(Column2, "Driver", 125, "Driver");

            GridViewColumnBuilder.ConfigureColumn(Column3, "Points", 55, "Stage Points");

            dataGridView.ColumnHeadersVisible = true;
            dataGridView.RowHeadersVisible = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.ReadOnly = true;
            dataGridView.AutoGenerateColumns = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.SelectionChanged += (s, e) => Grid.ClearSelection();

            return dataGridView;
        }

        #endregion
    }
}