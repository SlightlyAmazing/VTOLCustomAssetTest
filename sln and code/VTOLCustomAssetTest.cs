using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VoxelTycoon;
using VoxelTycoon.AssetLoading;
using VoxelTycoon.Buildings;
using VoxelTycoon.Buildings.AssetLoading;
using VoxelTycoon.Cities;
using VoxelTycoon.Modding;
using VoxelTycoon.Notifications;
using VoxelTycoon.Serialization;
using VoxelTycoon.Researches;
using VoxelTycoon.UI;
using VoxelTycoon.Game.UI;
using VoxelTycoon.Game.UI.ModernUI;
using VoxelTycoon.Tools;
using VoxelTycoon.Tools.Builder;
using VoxelTycoon.Tools.Remover;
using VoxelTycoon.Tools.Remover.Handlers;
using VoxelTycoon.Tracks;
using VoxelTycoon.Tracks.Rails;
using HarmonyLib;
using VTOL.BuildingUtils;

namespace VTOL_custom_asset_test
{
    public class VTOLCustomAssetTest : Mod
    {
        protected override void Initialize()
        {
            AssetLibrary.Current.RegisterHandler(new WaterAssetHandler());
            RemoverHandlerManager.Current.RegisterHandler(new WaterRemoverHandler());
            BuilderToolManager.Current.Register<WaterRecipe>((WaterRecipe x) => new WaterBuilderTool(x));
        }
        protected override void OnGameStarted()
        {
            UIManager.Current.SetTool(BuilderToolManager.Current.GetTool(BuildingRecipeManager.Current.Get(AssetLibrary.Current.GetAssetId("water/still_water.water"))));
        }
    }
    public class Water : Building
    {
        public new WaterSharedData SharedData
        {
            get
            {
                return (WaterSharedData)this._sharedData;
            }
            set
            {
                this._sharedData = value;
            }
        }
    }
    public class WaterAssetHandler : AssetHandler
    {
        public override string Extension
        {
            get
            {
                return "water";
            }
        }
        protected override object Import(AssetInfo assetInfo)
        {
            Tuple<Water, WaterAssetSurrogate> tuple = VTOLBuildingUtils.ImportBuilding<Water,WaterSharedData,WaterAssetSurrogate,WaterRecipe>(assetInfo);
            tuple.Item1.SharedData.FlowSpeed = tuple.Item2.FlowSpeed;
            tuple.Item1.SharedData.RainSpeed = tuple.Item2.RainSpeed;
            tuple.Item1.SharedData.SeepSpeed = tuple.Item2.SeepSpeed;
            return tuple.Item1;
        }
    }
    public class VTOLAssetSurrogate : BuildingAssetSurrogate
    {
        public string ResearchUri { get; set; }
    }
    public class WaterAssetSurrogate: VTOLAssetSurrogate
    {
        public float FlowSpeed { get; set; }
        public float SeepSpeed { get; set; }
        public float RainSpeed { get; set; }
    }
    public class WaterSharedData : BuildingSharedData
    {
        public float FlowSpeed { get; set; }
        public float SeepSpeed { get; set; }
        public float RainSpeed { get; set; }
    }
    public class WaterRemoverHandler : VTOLBuildingRemoverHandler<Water>
    {
        protected override bool CanRemoveInternal(List<Water> targets, out string reason)
        {
            foreach (Water water in targets)
            {
                Debug.Log(water.ToString() + " : ");
                reason = null;
                return false;
            }
            return base.CanRemoveInternal(targets, out reason);
        }
    }
    public class WaterRecipe : BuildingRecipe
    {
    }
    public class WaterBuilderTool : VTOLBuilderTool<WaterRecipe,Water>
    {
        public WaterBuilderTool(WaterRecipe recipe) : base(recipe)
        {
        }
    }
}
