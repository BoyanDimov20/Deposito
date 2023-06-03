import { useState } from 'react'
import './App.css'
import { Group, Radio, Select, TextInput } from '@mantine/core'

function App() {
  const [value, setValue] = useState('standard');

  return (
    <>
      <div style={{ border: '1px solid gray', borderRadius: '20px', padding: '20px' }}>
        <h3 style={{ marginBottom: '20px', textAlign: 'center' }}>Намери депозит</h3>
        <Radio.Group
          left={'auto'}
          value={value}
          onChange={setValue}
          label="ВИД ДЕПОЗИТ"
          withAsterisk
        >
          <Radio style={{ margin: '10px' }} value="standard" label="СТАНДАРТЕН СРОЧЕН ДЕПОЗИТ" />
          <Radio style={{ margin: '10px' }} value="monthly" label="ДЕПОЗИТ С МЕСЕЧНИ ВНОСКИ" />
        </Radio.Group>
        <div>
          <TextInput
            placeholder="Въведи размер"
            label="Размер на депозит"
            radius="md"
            size="md"
            type="number"
            withAsterisk
            style={{ marginTop: '20px' }}
          />

          <Select
            label="Валута"
            placeholder="Избери валута"
            data={[
              { value: 'bgn', label: 'BGN' },
              { value: 'eur', label: 'EUR' },
              { value: 'usd', label: 'USD' },
              { value: 'gbp', label: 'GBP' },
            ]}
            style={{ marginTop: '20px' }}
          />
        </div>
      </div>
    </>
  )
}

export default App
