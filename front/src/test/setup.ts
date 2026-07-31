import '@testing-library/jest-dom/vitest'
import {afterEach} from 'vitest'
import {cleanup} from '@testing-library/react'

// Очистка DOM после каждого теста (globals выключены, авто-cleanup не срабатывает)
afterEach(() => cleanup())
