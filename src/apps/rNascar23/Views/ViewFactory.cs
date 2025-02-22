﻿using rNascar23.Common;
using rNascar23.CustomViews;
using rNascar23.Logic;
using System.Collections.Generic;
using System.Drawing;

namespace rNascar23.Views
{
    internal static class ViewFactory
    {
        #region enums

        private enum ValueFormats
        {
            N0Format,
            N2Format,
            N3Format
        }

        #endregion

        #region fields

        private static readonly IDictionary<GridViewTypes, GridViewBase> _views = new Dictionary<GridViewTypes, GridViewBase>();

        #endregion

        #region public

        public static GridViewBase GetLapLeadersGridView()
        {
            var view = GetView(GridViewTypes.LapLeaders, "Lap Leaders");

            return view;
        }

        public static GridViewBase GetFastestLapsGridView(SpeedTimeType displayType)
        {
            var displayUnits = displayType == SpeedTimeType.MPH ? " (M.P.H.)" :
                displayType == SpeedTimeType.Seconds ? " (Lap Time)" :
                string.Empty;

            var view = GetView(GridViewTypes.FastestLaps, $"Fastest Laps{displayUnits}", ValueFormats.N2Format);

            return view;
        }

        public static GridViewBase GetMoversGridView()
        {
            var view = GetView(GridViewTypes.Movers, "Movers (Since Last Flag)");

            return view;
        }

        public static GridViewBase GetFallersGridView()
        {
            var view = GetView(GridViewTypes.Fallers, "Fallers (Since Last Flag)");

            return view;
        }

        public static GridViewBase GetNLapsGridView(NLapsViewTypes viewType, SpeedTimeType displayType)
        {
            var scale = displayType == SpeedTimeType.MPH ? " (Avg M.P.H.)" : " (Avg Lap)";

            string title;

            GridViewTypes gridViewType;

            switch (viewType)
            {
                case NLapsViewTypes.Best5Laps:
                    title = $"Best 5 Laps{scale}";
                    gridViewType = GridViewTypes.Best5Laps;
                    break;
                case NLapsViewTypes.Best10Laps:
                    title = $"Best 10 Laps{scale}";
                    gridViewType = GridViewTypes.Best10Laps;
                    break;
                case NLapsViewTypes.Best15Laps:
                    title = $"Best 15 Laps{scale}";
                    gridViewType = GridViewTypes.Best15Laps;
                    break;
                case NLapsViewTypes.Last5Laps:
                    title = $"Last 5 Laps{scale}";
                    gridViewType = GridViewTypes.Last5Laps;
                    break;
                case NLapsViewTypes.Last10Laps:
                    title = $"Last 10 Laps{scale}";
                    gridViewType = GridViewTypes.Last10Laps;
                    break;
                case NLapsViewTypes.Last15Laps:
                    title = $"Last 15 Laps{scale}";
                    gridViewType = GridViewTypes.Last15Laps;
                    break;
                default:
                    title = $"Best Laps{scale}";
                    gridViewType = GridViewTypes.Best5Laps;
                    break;
            }

            var view = GetView(gridViewType, title, ValueFormats.N2Format);

            return view;
        }

        public static GridViewBase GetDriverPointsGridView()
        {
            var view = GetView(GridViewTypes.DriverPoints, "Driver Points");

            return view;
        }

        public static GridViewBase GetStagePointsGridView()
        {
            var view = GetView(GridViewTypes.StagePoints, "Stage Points");

            return view;
        }

        public static GridViewBase GetFlagsGridView()
        {
            var view = GetView(GridViewTypes.Flags, "Flags");

            return view;
        }

        public static GridViewBase GetKeyMomentsGridView()
        {
            var view = GetView(GridViewTypes.KeyMoments, "Key Moments");

            return view;
        }

        #endregion

        #region private

        private static GridViewBase GetView(
            GridViewTypes viewType,
            string title,
            ValueFormats format = ValueFormats.N0Format,
            bool positionVisible = false)
        {
            if (_views.ContainsKey(viewType))
            {
                var cachedView = _views[viewType];

                if (cachedView.IsDisposed)
                {
                    _views.Remove(viewType);
                }
                else
                {
                    if (cachedView.Parent != null)
                    {
                        cachedView.Parent = null;
                    }
                    return cachedView;
                }
            }

            GridViewBase view;

            if (viewType == GridViewTypes.Flags)
                view = new FlagsView();
            else if (viewType == GridViewTypes.KeyMoments)
                view = new KeyMomentsView();
            else
                view = new GenericGridView();

            view.GridViewType = viewType;
            view.Settings.Title = title;
            view.Settings.Columns[0].Visible = positionVisible;

            switch (format)
            {
                case ValueFormats.N0Format:
                    view.Settings.Columns[2].Format = "N0";
                    view.Settings.Columns[2].Width = GridViewBase.N0Width;
                    break;
                case ValueFormats.N2Format:
                    view.Settings.Columns[2].Format = "N2";
                    view.Settings.Columns[2].Width = GridViewBase.N2Width;
                    break;
                case ValueFormats.N3Format:
                    view.Settings.Columns[2].Format = "N3";
                    view.Settings.Columns[2].Width = GridViewBase.N3Width;
                    break;
                default:
                    break;
            }

            view.Settings.ViewHeight = 250;
            view.Settings.GridHeight = view.Settings.ViewHeight - view.TitleLabel.Height;

            ApplyGridSpecificSettings(viewType, view);

            view.AutoSizeGrid();

            _views.Add(viewType, view);

            return view;
        }

        private static void ApplyGridSpecificSettings(GridViewTypes viewType, GridViewBase view)
        {
            switch (viewType)
            {
                case GridViewTypes.DriverPoints:
                    view.Settings.MaxRows = 36;
                    view.Settings.TitleBackColor = Color.Orange;
                    view.Settings.TitleForeColor = Color.White;
                    view.Settings.ViewHeight = 210;
                    break;

                case GridViewTypes.FastestLaps:
                    view.Settings.TitleBackColor = Color.LightSteelBlue;
                    view.Settings.TitleForeColor = Color.Black;
                    view.Settings.ViewHeight = 210;
                    break;

                case GridViewTypes.LapLeaders:
                    view.Settings.TitleBackColor = Color.DarkBlue;
                    view.Settings.TitleForeColor = Color.White;
                    break;

                case GridViewTypes.StagePoints:
                    view.Settings.TitleBackColor = Color.Teal;
                    view.Settings.TitleForeColor = Color.White;
                    view.Settings.ViewHeight = 210;
                    break;

                case GridViewTypes.Best5Laps:
                case GridViewTypes.Best10Laps:
                case GridViewTypes.Best15Laps:
                    view.Settings.Columns[1].Width = 120; // driver name column
                    view.Settings.TitleBackColor = Color.CornflowerBlue;
                    view.Settings.TitleForeColor = Color.Black;
                    break;

                case GridViewTypes.Last5Laps:
                case GridViewTypes.Last10Laps:
                case GridViewTypes.Last15Laps:
                    view.Settings.Columns[1].Width = 120; // driver name column
                    view.Settings.TitleBackColor = Color.RoyalBlue;
                    view.Settings.TitleForeColor = Color.White;
                    break;

                case GridViewTypes.Movers:
                    view.Settings.MaxRows = 5;
                    view.Settings.ViewHeight = 140;

                    view.Settings.TitleBackColor = Color.Green;
                    view.Settings.TitleForeColor = Color.White;
                    break;

                case GridViewTypes.Fallers:
                    view.Settings.MaxRows = 5;
                    view.Settings.ViewHeight = 140;

                    view.Settings.TitleBackColor = Color.DarkRed;
                    view.Settings.TitleForeColor = Color.White;
                    break;

                case GridViewTypes.Flags:

                    view.Settings = new GridSettings()
                    {
                        MaxRows = null,
                        TitleBackColor = FlagUiColors.Yellow,
                        TitleForeColor = Color.Black,
                        Title = "Flags",
                        HideColumnHeaders = false,
                        HideRowSelector = true,
                        Columns = new List<GridColumnSettings>()
                        {
                            new GridColumnSettings()
                            {
                                Index= 0,
                                DataProperty = "State",
                                DisplayIndex= 0,
                                Visible =false,
                                Width = 60
                            },
                            new GridColumnSettings()
                            {
                                Index= 1,
                                DataProperty = "LapNumber",
                                DisplayIndex= 1,
                                HeaderTitle ="Lap",
                                Visible =true,
                                Width = 38
                            },
                            new GridColumnSettings()
                            {
                                Index= 2,
                                DataProperty = "Comment",
                                DisplayIndex= 2,
                                HeaderTitle ="Caution For",
                                Visible =true,
                                Width = 150,
                            },
                            new GridColumnSettings()
                            {
                                Index= 3,
                                DataProperty = "Beneficiary",
                                DisplayIndex= 3,
                                HeaderTitle ="Pass",
                                Visible =true,
                                Width = 38,
                            },
                            new GridColumnSettings()
                            {
                                Index= 4,
                                DataProperty = "TimeOfDayOs",
                                DisplayIndex= 4,
                                HeaderTitle ="Time",
                                Visible =true,
                                Width = 70,
                                Format = "h:mm tt"
                            }
                        },
                        SortOrderField = "LapNumber",
                        SortOrder = (int)GridViewBase.GridSortOrder.Ascending
                    };
                    break;

                case GridViewTypes.KeyMoments:

                    view.Settings = new GridSettings()
                    {
                        MaxRows = null,
                        TitleBackColor = Color.Magenta,
                        TitleForeColor = Color.Black,
                        Title = "Key Moments",
                        HideColumnHeaders = false,
                        HideRowSelector = true,
                        Columns = new List<GridColumnSettings>()
                        {
                            new GridColumnSettings()
                            {
                                Index= 0,
                                DataProperty = "NoteId",
                                DisplayIndex= 0,
                                HeaderTitle ="NoteId",
                                Visible =false,
                                Width = 0
                            },
                            new GridColumnSettings()
                            {
                                Index= 1,
                                DataProperty = "LapNumber",
                                DisplayIndex= 1,
                                HeaderTitle ="Lap",
                                Visible =true,
                                Width = 38
                            },
                            new GridColumnSettings()
                            {
                                Index= 2,
                                DataProperty = "FlagState",
                                DisplayIndex= 2,
                                HeaderTitle ="State",
                                Visible =false,
                                Width = 0
                            },
                            new GridColumnSettings()
                            {
                                Index= 3,
                                DataProperty = "Note",
                                DisplayIndex= 3,
                                Visible =true,
                                Width = 280,
                            }
                        },
                        SortOrderField = "NoteId",
                        SortOrder = (int)GridViewBase.GridSortOrder.Ascending
                    };
                    break;

                default:
                    break;
            }
        }

        #endregion
    }
}
