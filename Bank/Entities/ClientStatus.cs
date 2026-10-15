namespace Bank;

/// <summary>
/// Статус клиента банка. Определяет размер комиссии за обслуживание,
/// процент на остаток и наличие кэшбэка.
/// </summary>
public enum ClientStatus
{
    /// <summary>
    /// Обычный клиент.
    /// </summary>
    Regular,

    /// <summary>
    /// Премиум-клиент.
    /// </summary>
    Premium,

    /// <summary>
    /// VIP-клиент.
    /// </summary>
    VIP
}