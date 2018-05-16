﻿using Pipliz;
using System.Collections.Generic;

namespace ExtendedAPI
    {
    [ModLoader.ModManager]
    public static class Autoloader
        {

        //Busca todos las clases con metatags y las anyade a su manager correspondiente
        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterModsLoaded, "Khanx.ExtendedAPI.ParseMods")]
        public static void AfterModsLoaded(List<ModLoader.ModDescription> assemblies)
            {
            foreach(var modAssembly in assemblies)
                {
                if(modAssembly.HasAssembly)
                    {
                    foreach(System.Type type in modAssembly.LoadedAssemblyTypes)
                        {
                        try
                            {
                            //Recipe
                            if(type.IsDefined(typeof(Recipes.AutoLoadRecipeAttribute), true))
                                {
                                Recipes.RecipeManager.Add(type);
                                }
                            }
                        catch(System.Exception e)
                            {
                            Log.WriteException("APIProvider threw exception parsing dll {0}, type {1}", e, System.IO.Path.GetFileName(modAssembly.LoadedAssembly.Location), type.FullName);
                            }
                        }
                    }
                }
            }

        //Anyade las investigaciones
        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterItemTypesDefined, "Khanx.ExtendedAPI.RegisterRecipes")]
        [ModLoader.ModCallbackDependsOn("pipliz.server.recipeplayerload")]  //After loading the NPC & player recipes
        //[ModLoader.ModCallback(ModLoader.EModCallbackType.AfterWorldLoad, "Khanx.ExtendedAPI.RegisterRecipes")]
        public static void RegisterAutoRecipes()
            {
            Recipes.RecipeManager.Register();
            }

        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnPlayerRecipeSettingChanged, "khanx.extendedapi.OnPlayerRecipeSettingChanged")]
        public static void OnPlayerRecipeSettingChanged(RecipeStorage.PlayerRecipeStorage storage, Recipe recipe, Box<RecipeStorage.RecipeSetting> recipeSetting)
            {
            Recipes.RecipeManager.OnPlayerRecipeSettingChanged(storage, recipe.Name, recipeSetting);
            }

        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnNPCCraftedRecipe, "khanx.extendedapi.OnPlayerRecipeSettingChanged")]
        public static void OnNPCCraftedRecipe(NPC.IJob job, Recipe recipe, List<InventoryItem> results)
            {
            Recipes.RecipeManager.OnNPCCraftedRecipe(job, recipe.Name, results);
            }

        }
    }