﻿using System;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Common.Gameplay.Users.Skills.Templates;
using Edelstein.Common.Util;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Attacking;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Attacking
{
    public class CalculatedDamage : ICalculatedDamage
    {
        private static readonly int RND_SIZE = 7;

        public uint InitSeed1 { get; private set; }
        public uint InitSeed2 { get; private set; }
        public uint InitSeed3 { get; private set; }

        private CRand32 RndGenForCharacter { get; set; }
        private CRand32 RndGenForMob { get; set; }

        private ITemplateRepository<SkillTemplate> _skillTemplates;

        public CalculatedDamage(ITemplateRepository<SkillTemplate> skillTemplates)
        {
            var random = new Random();

            SetSeed((uint)random.Next(), (uint)random.Next(), (uint)random.Next());

            _skillTemplates = skillTemplates;
        }

        public void SetSeed(uint s1, uint s2, uint s3)
        {
            InitSeed1 = s1;
            InitSeed2 = s2;
            InitSeed3 = s3;

            s1 |= 0x100000;
            s2 |= 0x1000;
            s3 |= 0x10;

            RndGenForCharacter = new CRand32(s1, s2, s3);
            RndGenForMob = new CRand32(s1, s2, s3);
        }

        public void SkipCalculationForCharacterDamage() => RndGenForCharacter.Next(new uint[RND_SIZE]);

        public ICalculatedDamageInfo[] CalculateCharacterPDamage(IAttackInfo info)
        {
            var random = new Rotational<uint>(new uint[RND_SIZE]);
            var skillTemplate = info.SkillID > 0 ? _skillTemplates.Retrieve(info.SkillID).Result : null;
            var skillLevelTemplate = info.SkillID > 0 ? skillTemplate.LevelData[info.SkillLevel] : null;
            var attackCount = skillLevelTemplate != null ? skillLevelTemplate.AttackCount : 1;
            var result = new ICalculatedDamageInfo[attackCount];

            RndGenForCharacter.Next(random.Array);

            for (var i = 0; i < attackCount; i++)
            {
                random.Next(); // if mob not invincible, calc miss
                random.Next();

                var damage = GetRandomInRange(random.Next(), info.User.Stats.DamageMin, info.User.Stats.DamageMax);
                var critical = false;

                if (skillLevelTemplate != null)
                    damage *= skillLevelTemplate.Damage / 100d;

                damage *= (100d - (info.Mob.Stats.PDR * info.User.Stats.IMDr / -100 + info.Mob.Stats.PDR)) / 100d;

                if (info.User.Stats.Cr > 0 && GetRandomInRange(random.Next(), 0.0, 100.0) <= info.User.Stats.Cr)
                {
                    var cd = GetRandomInRange(random.Next(), info.User.Stats.CDMin, info.User.Stats.CDMax) / 100d;

                    critical = true;
                    damage += (int)(damage * cd);
                }

                damage *= 0.9; // TODO unsure

                result[i] = new CalculatedDamageInfo((int)damage, critical);
            }

            return result;
        }

        public ICalculatedDamageInfo[] CalculateCharacterMDamage(IAttackInfo info)
        {
            var random = new Rotational<uint>(new uint[RND_SIZE]);

            RndGenForCharacter.Next(random.Array);

            return Array.Empty<ICalculatedDamageInfo>();
        }

        public void SkipCalculationForMobDamage() => RndGenForMob.Next(new uint[RND_SIZE]);

        public int CalculateMobPDamage(IMobAttackInfo info)
        {
            return 0;
        }

        public int CalculateMobMDamage(IMobAttackInfo info)
        {
            return 0;
        }


        public double GetRandomInRange(uint rand, double f0, double f1)
        {
            if (f1 != f0)
            {
                if (f0 > f1)
                {
                    var tmp = f1;
                    f0 = f1;
                    f1 = tmp;
                }

                return f0 + rand % 10000000 * (f1 - f0) / 9999999.0;
            }
            else return f0;
        }
    }
}
