import {Timeline, Text} from "@mantine/core";
import {PayoutType, useCalculatedDeposit} from "../services/useCalculatedDeposit";


function translatePayoutType(payoutType: PayoutType) {
    switch (payoutType) {
        case 1:
            return 'ежемесечно';
        case 2:
            return 'годишно';
        case 3:
            return 'в края на периода';
        default:
            return 'ежемесечно';
    }
}

export default function Deposit() {

    const deposit = useCalculatedDeposit();

    if (!deposit) {
        return <></>;
    }
    return (
        <div style={{
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            border: '1px solid gray',
            borderRadius: '20px',
            padding: '10px 30px 30px 30px'
        }}>
            <div style={{marginBottom: '10px', fontSize: '20px', padding: '20px'}}>Изчисляване на депозит</div>
            <div style={{fontSize: 'calc(1.65625rem + 4.875vw)'}}>{deposit.percent}%</div>
            <div>лихвен процент</div>
            <a href={`/api/Deposit/GenerateExcel?amount=${deposit.depositedAmount}&interest=${deposit.percent}&period=${deposit.period}&payoutType=${deposit.payoutType}`}
               download="Разплащателен план.xls">Разплащателен план</a>
            <Timeline active={0} bulletSize={24} lineWidth={2} style={{marginTop: '20px'}}>
                <Timeline.Item title="Заявка за депозит">
                    <Text color="dimmed" size="sm">Изпрати ни заявка за депозит</Text>
                </Timeline.Item>

                <Timeline.Item title="Изчакай удобрение ">
                    <Text color="dimmed" size="sm">Служител ще се свърже с вас при удобрение</Text>
                </Timeline.Item>

                <Timeline.Item title={`Депозирай сумата от ${deposit.depositedAmount} ${deposit.currency}`}
                               lineVariant="dashed">
                    <Text color="dimmed" size="sm">За период от {deposit.period} месеца</Text>
                </Timeline.Item>

                <Timeline.Item title={
                    <>
                        Изплащане
                        <Text fw={500} variant="gradient"
                              italic={true}
                              component="span"
                              inherit> ({translatePayoutType(deposit.payoutType)})</Text></>
                }>
                    <Text color="dimmed" size="sm">Сумата се изплаща : <Text fw={700} variant="gradient"
                                                                             component="span"
                                                                             inherit>{deposit.amountAfterDeposit.toFixed(2)} {deposit.currency}</Text></Text>
                </Timeline.Item>
            </Timeline>
        </div>
    );
}