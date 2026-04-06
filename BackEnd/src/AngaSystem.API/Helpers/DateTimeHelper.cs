using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngaSystem.API.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime AdicionaHorasDiaAtualMilissegundos(long valorMilissegundos)
        {
            if (valorMilissegundos > 0)
            {
                var longToHoras = new DateTimeOffset(valorMilissegundos, TimeSpan.Zero);

                return DateTime.Today.AddHours(longToHoras.Hour).AddMinutes(longToHoras.Minute);
            }
            else
            {
                return DateTime.Today;
            }
        }

        public static bool ValidaDataEHoraEntre(DateTime dataHoraPesquisa, DateTime dataHoraInicial, DateTime dataHoraFinal)
        {
            if (dataHoraPesquisa.Ticks >= dataHoraInicial.Ticks && dataHoraPesquisa.Ticks <= dataHoraFinal.Ticks)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}