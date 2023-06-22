import { Timeline, Text } from "@mantine/core";
import { useCalculatedDeposit } from "../services/useCalculatedDeposit";


function translatePayoutType(payoutType: number) {
    switch (payoutType) {
        case 0:
            return 'на определената дата';
        case 1:
            return 'предварително';
        case 2:
            return 'ежемесечно';
        case 3:
            return 'в края на периода';
        default:
            break;
    }
}

export default function Deposit() {

    const deposit = useCalculatedDeposit();

    if(!deposit) {
        return <></>;
    }
    return (
        <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', border: '1px solid gray', borderRadius: '20px', padding: '10px 30px 30px 30px' }}>
            <div style={{ marginBottom: '10px', fontSize: '20px', padding: '20px' }}>Изчисляване на депозит</div>
            <div style={{ fontSize: 'calc(1.65625rem + 4.875vw)' }}>{deposit.percent}%</div>
            <div>лихвен процент</div>
            <Timeline active={0} bulletSize={24} lineWidth={2} style={{ marginTop: '20px' }}>
                <Timeline.Item title="Заявка за депозит">
                    <Text color="dimmed" size="sm">Изпрати ни заявка за депозит</Text>
                </Timeline.Item>

                <Timeline.Item title="Изчакай удобрение ">
                    <Text color="dimmed" size="sm">Служител ще се свърже с вас при удобрение</Text>
                </Timeline.Item>

                <Timeline.Item title={`Депозирай сумата от ${deposit.depositedAmount} ${deposit.currency}`} lineVariant="dashed">
                    <Text color="dimmed" size="sm">За период от {deposit.period} месеца</Text>
                </Timeline.Item>

                <Timeline.Item title="Изплащане">
                    <Text color="dimmed" size="sm">Общата сума в края на период: <Text fw={700} variant="gradient" component="span" inherit>{deposit.amountAfterDeposit} {deposit.currency}</Text></Text>
                </Timeline.Item>
            </Timeline>
        </div>
    );
}