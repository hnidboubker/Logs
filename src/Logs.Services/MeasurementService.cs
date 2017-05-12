﻿using Logs.Data.Contracts;
using Logs.Factories;
using Logs.Models;
using Logs.Services.Contracts;
using System;

namespace Logs.Services
{
    public class MeasurementService : IMeasurementService
    {
        private readonly IRepository<Measurement> repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly INutritionService nutritionService;
        private readonly IMeasurementFactory factory;

        public MeasurementService(IRepository<Measurement> repository,
            IUnitOfWork unitOfWork,
            INutritionService nutritionService,
            IMeasurementFactory factory)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (unitOfWork == null)
            {
                throw new ArgumentNullException(nameof(unitOfWork));
            }

            if (nutritionService == null)
            {
                throw new ArgumentNullException(nameof(nutritionService));
            }

            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.nutritionService = nutritionService;
            this.factory = factory;
        }

        public Measurement UpdateMeasurement(int id, string userId, DateTime date, int height, double weightKg,
            double bodyFatPercent, int chest, int shoulders, int forearm, int arm, int waist,
            int hips, int thighs, int calves, int neck, int wrist, int ankle)
        {
            var measurement = this.EditMeasurement(id, height, weightKg,
             bodyFatPercent, chest, shoulders, forearm, arm, waist,
             hips, thighs, calves, neck, wrist, ankle);

            if (measurement == null)
            {
                var entry = this.nutritionService.GetEntryByDate(userId, date);

                if (entry == null)
                {
                    entry = this.nutritionService.CreateNutritionEntry(userId, date);
                }

                measurement = this.CreateMeasurement(height, weightKg,
             bodyFatPercent, chest, shoulders, forearm, arm, waist,
             hips, thighs, calves, neck, wrist, ankle, entry.NutritionEntryId);
            }

            return measurement;
        }

        public Measurement EditMeasurement(int id, int height, double weightKg, double bodyFatPercent, int chest, int shoulders, int forearm, int arm, int waist, int hips, int thighs, int calves, int neck, int wrist, int ankle)
        {
            throw new NotImplementedException();
        }

        public Measurement CreateMeasurement(int height, double weightKg, double bodyFatPercent, int chest, int shoulders, int forearm, int arm, int waist, int hips, int thighs, int calves, int neck, int wrist, int ankle, int entryId)
        {
            throw new NotImplementedException();
        }
    }
}
